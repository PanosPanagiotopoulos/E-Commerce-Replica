using E_Commerce_Application_API.Models;

namespace E_Commerce_Application_API.SeedHelpers
{
    public class FakeDbDataGenerator
    {
        // Function to initialize Users
        public static List<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Firstname = "Alice",
                    Lastname = "Johnson",
                    Phonenumber = "555-1234",
                    Email = "alice.johnson@example.com",
                    Password = "SecurePass!1A",
                    ShoppingCart = new ShoppingCart()
                },
                new User
                {
                    Firstname = "Bob",
                    Lastname = "Smith",
                    Phonenumber = "555-5678",
                    Email = "bob.smith@example.com",
                    Password = "SecurePass!1B",
                    ShoppingCart = new ShoppingCart()
                },
                new User
                {
                    Firstname = "Carol",
                    Lastname = "Williams",
                    Phonenumber = "555-9012",
                    Email = "carol.williams@example.com",
                    Password = "SecurePass!1C",
                    ShoppingCart = new ShoppingCart()
                }
            };

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

        // Function to initialize Products
        public static List<Product> GetProducts()
        {
            var blobUrls = new List<string>
            {
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThRXuidqRiC6fOuxBYenmi8knrzZ1qLxNjcchJVm31NKeelz_iVAXQ3llZhspgVR8sOXs&usqp=CAU",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRKEskkEsgaaMnUV6zBHUvPreeAMP2vxv1E2A&usqp=CAU",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSF9M_-P3YiCb4xbfeKG87GH_oI6raUliRViQ&usqp=CAU"
            };

            var products = new List<Product>
            {
                new Product
                {
                    Pid = "PROD001",
                    Title = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with adjustable DPI.",
                    Price = 29.99m,
                    Category = "Electronics",
                    ShippingCost = 4.99m
                },
                new Product
                {
                    Pid = "PROD002",
                    Title = "Mechanical Keyboard",
                    Description = "RGB backlit mechanical keyboard with blue switches.",
                    Price = 79.99m,
                    Category = "Electronics",
                    ShippingCost = 7.99m
                },
                new Product
                {
                    Pid = "PROD003",
                    Title = "Noise-Canceling Headphones",
                    Description = "Over-ear headphones with active noise cancellation.",
                    Price = 199.99m,
                    Category = "Electronics",
                    ShippingCost = 9.99m
                }
            };

            // Assign 3 images to each product
            foreach (var product in products)
            {
                product.Images = new List<ImageFile>();
                for (int i = 0; i < 3; i++)
                {
                    var image = new ImageFile
                    {
                        Filename = "somefile.png",
                        BlobURL = blobUrls[i % blobUrls.Count],
                        Product = product
                    };
                    product.Images.Add(image);
                }
            }

            return products;
        }

        // Function to initialize MethodProducts
        public static List<MethodProducts> GetMethodProducts(List<PaymentMethod> paymentMethods, List<Product> products)
        {
            var methodProducts = new List<MethodProducts>
            {
                // Associate "Cash" with the first product
                new MethodProducts
                {
                    PaymentMethod = paymentMethods.First(pm => pm.method == "Cash"),
                    Product = products[0]
                },
                // Associate "Card" with the first and second products
                new MethodProducts
                {
                    PaymentMethod = paymentMethods.First(pm => pm.method == "Card"),
                    Product = products[0]
                },
                new MethodProducts
                {
                    PaymentMethod = paymentMethods.First(pm => pm.method == "Card"),
                    Product = products[1]
                },
                // Associate "Paypal" with the second and third products
                new MethodProducts
                {
                    PaymentMethod = paymentMethods.First(pm => pm.method == "Paypal"),
                    Product = products[1]
                },
                new MethodProducts
                {
                    PaymentMethod = paymentMethods.First(pm => pm.method == "Paypal"),
                    Product = products[2]
                }
            };

            return methodProducts;
        }

        // Function to initialize CartProducts
        public static List<CartProducts> GetCartProducts(List<User> users, List<Product> products)
        {
            var cartProducts = new List<CartProducts>
            {
                // User 1 has one product in the cart
                new CartProducts
                {
                    ShoppingCart = users[0].ShoppingCart,
                    Product = products[0],
                    Quantity = 2
                },
                // User 2 has two products in the cart
                new CartProducts
                {
                    ShoppingCart = users[1].ShoppingCart,
                    Product = products[1],
                    Quantity = 1
                },
                new CartProducts
                {
                    ShoppingCart = users[1].ShoppingCart,
                    Product = products[2],
                    Quantity = 1
                }
                // User 3's cart is empty
            };

            return cartProducts;
        }

    }
}
