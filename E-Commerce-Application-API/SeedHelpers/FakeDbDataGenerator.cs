using Amazon.S3;
using Amazon.S3.Model;
using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.SeedHelpers
{
    public class FakeDbDataGenerator
    {
        // Function to initialize Users
        public static List<User> GetUsers()
        {
            var users = new List<User>();
            var random = new Random();

            var firstNames = new List<string> { "Alice", "Bob", "Carol", "David", "Eve", "Frank", "Grace", "Heidi", "Ivan", "Judy", "Karl", "Liam", "Mallory", "Niaj", "Olivia", "Peggy", "Quentin", "Rupert", "Sybil", "Trent" };
            var lastNames = new List<string> { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };

            for (int i = 0; i < 20; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Count)];
                var lastName = lastNames[random.Next(lastNames.Count)];
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(1000)}@example.com";
                var phoneNumber = $"555-{random.Next(1000, 9999)}";
                var password = $"Password{random.Next(1000)}!";

                var user = new User
                {
                    Firstname = firstName,
                    Lastname = lastName,
                    Phonenumber = phoneNumber,
                    Email = email,
                    Password = password, // Will hash this below
                    RegisteredDate = DateTime.UtcNow,
                    ShoppingCart = new ShoppingCart()
                };

                // Hash the password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                users.Add(user);
            }

            return users;
        }

        // Function to initialize PaymentMethods
        public static List<PaymentMethod> GetPaymentMethods()
        {
            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod { method = "Cash" },
                new PaymentMethod { method = "Card" },
                new PaymentMethod { method = "Paypal" }
            };

            return paymentMethods;
        }

        // Private class for S3 object data
        private class S3ObjectData
        {
            public string Filename { get; set; }
            public string BlobUrl { get; set; }

            public S3ObjectData(string filename, string blobUrl)
            {
                this.Filename = filename;
                this.BlobUrl = blobUrl;
            }
        }

        // Function to initialize Products
        public static async Task<List<Product>> GetProducts(IAmazonS3 s3Client, string bucketName)
        {
            Dictionary<string, List<S3ObjectData>> productImages = new Dictionary<string, List<S3ObjectData>>();
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketName
                };

                ListObjectsV2Response response;
                do
                {
                    response = await s3Client.ListObjectsV2Async(request);

                    foreach (S3Object entry in response.S3Objects)
                    {
                        // Get the filename of this object
                        string filename = Path.GetFileName(entry.Key);

                        // Get the product id that this image references to
                        string productId = Path.GetFileNameWithoutExtension(entry.Key).Split('_')[0]; // Assuming format is PRODUCTID_something

                        // Generate the full URL for each object in the bucket
                        string blobUrl = $"https://{bucketName}.s3.amazonaws.com/{entry.Key}";

                        if (!productImages.ContainsKey(productId))
                        {
                            productImages.Add(productId, new List<S3ObjectData>());
                        }

                        productImages[productId].Add(new S3ObjectData(filename, blobUrl));
                    }

                    // Set continuation token for the next request
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching files from S3: " + ex.Message);
                return new List<Product>();
            }

            List<Product> products = new List<Product>();
            var random = new Random();

            // Generate random product data for each productId
            foreach (var productEntry in productImages)
            {
                string productId = productEntry.Key;

                // Generate random title, description, etc.
                string[] adjectives = { "Amazing", "Incredible", "Fantastic", "Innovative", "Portable", "Compact", "Advanced", "Durable", "High-Quality", "Affordable" };
                string[] nouns = { "Device", "Gadget", "Tool", "Instrument", "Accessory", "Equipment", "Product", "Item", "Appliance", "Hardware" };
                string[] categories = { "Shoes", "Trousers", "Shirts", "Pants", "Hats", "T-Shirts", "Socks", "Jeans" };

                string title = $"{adjectives[random.Next(adjectives.Length)]} {nouns[random.Next(nouns.Length)]}";
                string description = $"This {title} is perfect for your needs. It features {random.Next(1, 10)} exciting functions.";
                decimal price = (decimal)(random.Next(1000, 100000) / 100.0); // Random price between 10.00 and 1000.00
                decimal shippingCost = (decimal)(random.Next(500, 5000) / 100.0); // Random shipping cost between 5.00 and 50.00
                string category = categories[random.Next(categories.Length)];

                var product = new Product
                {
                    Pid = productId,
                    Title = title,
                    Description = description,
                    Price = price,
                    Category = category,
                    ShippingCost = shippingCost,
                    Images = productEntry.Value.Select(img => new ImageFile
                    {
                        Filename = img.Filename,
                        BlobURL = img.BlobUrl
                    }).ToList()
                };

                products.Add(product);
            }

            return products;
        }

        // Function to initialize MethodProducts
        public static List<MethodProducts> GetMethodProducts(List<PaymentMethod> paymentMethods, List<Product> products)
        {
            var methodProducts = new List<MethodProducts>();
            var random = new Random();

            foreach (var product in products)
            {
                // Randomly select 2-3 payment methods for each product
                int numberOfMethods = random.Next(2, 4); // 2 or 3 methods
                var selectedMethods = paymentMethods.OrderBy(x => random.Next()).Take(numberOfMethods).ToList();

                foreach (var method in selectedMethods)
                {
                    methodProducts.Add(new MethodProducts
                    {
                        PaymentMethod = method,
                        Product = product
                    });
                }
            }

            return methodProducts;
        }

        // Function to initialize CartProducts
        public static List<CartProducts> GetCartProducts(List<User> users, List<Product> products)
        {
            var cartProducts = new List<CartProducts>();
            var random = new Random();

            foreach (var user in users)
            {
                // Randomly decide how many products are in this user's cart
                int productsInCart = random.Next(0, 6); // 0 to 5 products

                // Randomly select products for this cart
                var selectedProducts = products.OrderBy(x => random.Next()).Take(productsInCart).ToList();

                foreach (var product in selectedProducts)
                {
                    cartProducts.Add(new CartProducts
                    {
                        ShoppingCart = user.ShoppingCart,
                        Product = product,
                        Quantity = random.Next(1, 5) // Quantity between 1 and 4
                    });
                }
            }

            return cartProducts;
        }
    }
}
