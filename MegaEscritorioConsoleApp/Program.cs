using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MegaEscritorioConsoleApp
{
    class Program
    {
        // Setup the initial variables for the base
        // of the program.
        double deskWidth,
               deskLength,
               deskArea,
               deskBasePrice,
               deskSurfaceAreaPrice,
               drawerQuantity,
               drawerPrice,
               deskMaterialPrice,
               rushOrderQuantity,
               rushOrderPrice,
               totalPrice;

        string deskMaterial, saveOrder, orderOutput;


        // users can specify the size(width and length) of the desk in inches,
        // users can choose between zero and seven drawers,
        // users can select between three surface materials(laminate, oak, or pine), and
        // users can choose rush order options of 3, 5, or 7 days given the normal production time is 14 days.

        static void Main()
        {
            // Display welcome message with instructions
            Console.WriteLine(
                "-----------------------------------------------------" +
                "\nCIT301C - MegaEscritorio Console Application" +
                "\nFollow the prompts to help select your desk" +
                "\npreferences and provide you with a quote." +
                "\n-----------------------------------------------------" +
                "\n\nPress any key to continue.");

            Console.ReadLine();

            // Set up program
            Program program = new Program();

            // Desk size
            program.requestDeskSize();
            program.calculateAreaPrice();

            // Drawer quantity
            program.requestDrawerQuantity();
            program.calculateDrawerPrice();

            // Material type
            program.requestDeskMaterial();
            program.calculateDeskMaterialPrice();

            // Rush order pricing
            int[,] rushOrderPricingArray = new int[3, 3];
            program.getRushOrderPricing(rushOrderPricingArray);
            program.requestRushOrderTimeframe();
            program.calculateRushOrderPrice(rushOrderPricingArray);

            // Total price
            program.calculateTotalPrice();

            // Save order
            program.requestFileOutput();
            program.saveFile();
        }

        // Request the desk size
        private double requestDeskSize()
        {
            // Request the width of the desk
            Console.WriteLine("\nHow wide do you want your desk (in inches)?");
            deskWidth = Convert.ToDouble(Console.ReadLine());

            // Request the length of the desk
            Console.WriteLine("\nHow long do you want your desk (in inches)?");
            deskLength = Convert.ToDouble(Console.ReadLine());

            deskArea = deskLength * deskWidth;

            // Debug
            // Console.WriteLine("Calculated desk area:" + deskArea);

            return deskArea;
        }

        // Calculate the area of the desk price
        // Base desk price is $200
        // If the surface area is over 1000 square inches
        // the desk price is $5 per square inch.
        public double calculateAreaPrice()
        {
            if (deskArea > 1000)
            {
                deskSurfaceAreaPrice = ((deskArea - 1000) * 5) + 200;
            } else
            {
                deskSurfaceAreaPrice = 200;
            }

            // Debug
            // Console.WriteLine("Calcualted desk price:" + deskSurfaceAreaPrice);

            return deskSurfaceAreaPrice;
        }

        // Request number of drawers
        private double requestDrawerQuantity()
        {
            // Request drawer quantity
            Console.WriteLine("\nHow many drawers would you like for this desk?");
            drawerQuantity = Convert.ToDouble(Console.ReadLine());

            return drawerQuantity;
        }

        // Calculate the price per drawer
        // Each drawer costs $50
        // A max of 7 drawers can be requested
        public double calculateDrawerPrice()
        {
            if (drawerQuantity > 0 && drawerQuantity < 8)
            {
                drawerPrice = drawerQuantity * 50;
            } else if (drawerQuantity == 0)
            {
                drawerPrice = 0;
            } else
            {
                Console.WriteLine("\nSorry. We can't add more than 7 drawers.");
                requestDrawerQuantity();
            }

            // Debug
            // Console.WriteLine("Calcualted drawer price:" + drawerPrice);

            return drawerPrice;
        }

        // Request desk materials:
        // You can choose laminate ($100), oak ($200), or pine ($50)
        private string requestDeskMaterial()
        {
            Console.WriteLine("\nWhich material would you like for the desk: Pine, Laminate, or Oak?");
            deskMaterial = Convert.ToString(Console.ReadLine());

            return deskMaterial;
        }

        // Calculate desk material price:
        // laminate ($100), oak ($200), or pine ($50)
        public double calculateDeskMaterialPrice()
        {
            if (deskMaterial == "Pine" || deskMaterial == "pine")
            {
                deskMaterialPrice = 50;
            } else if (deskMaterial == "Laminate" || deskMaterial == "laminate")
            {
                deskMaterialPrice = 100;
            } else if (deskMaterial == "Oak" || deskMaterial == "oak")
            {
                deskMaterialPrice = 200;
            } else
            {
                Console.WriteLine("\nSorry. The material you asked for isn't an option.");
                requestDeskMaterial();
            }

            // Debug
            // Console.WriteLine("Material Price:" + deskMaterialPrice);

            return deskMaterialPrice;
        }

        // Read the rush order text file and map it
        // to a 2d array
        public void getRushOrderPricing(int[,] rushOrderArray)
        {
            try
            {
                string[] rushOrderPricingList = File.ReadAllLines("rushOrderPricing.txt");
                int rushOrderCount = 0;
                for (int i = 0; i < rushOrderArray.GetLength(0); i++)
                {
                    for (int j = 0; j < rushOrderArray.GetLength(1); j++)
                    {
                        rushOrderArray[i, j] = int.Parse(rushOrderPricingList[rushOrderCount]);
                        rushOrderCount++;
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        // Request rush order date
        // Default is 14 days. Rush orders can be 3, 5, or 7 days
        private double requestRushOrderTimeframe ()
        {
            Console.WriteLine("\nNormal production time is 14 days." + 
                              "\nRush order options are available for 3, 5, or 7 days." +
                              "\nEnter 3, 5, or 7 to speed up your order." + 
                              "\nEnder 0 to stick with normal production times.");
            rushOrderQuantity = Convert.ToDouble(Console.ReadLine());

            return rushOrderQuantity;
        }
        public double calculateRushOrderPrice(int[,] rushOrderPricingArray)
        {
            int i = 0, j = 0;

            if (rushOrderQuantity == 0)
            {
                return 0;
            } else
            {
                // Pricing based on desk area
                if (deskArea <= 1000)
                {
                    j = 0;
                } else if (deskArea > 1000 && deskArea >= 2000)
                {
                    j = 1;
                } else
                {
                    j = 2;
                }

                // Pricing based on rush order timeline
                if (rushOrderQuantity == 3)
                {
                    i = 0;
                } else if (rushOrderQuantity == 5)
                {
                    i = 1;
                } else
                {
                    i = 2;
                }
                
                rushOrderPrice = rushOrderPricingArray[i, j];

                // Debug
                // Console.WriteLine("Rush Price:" + rushOrderPrice);

                return rushOrderPrice;
            }
        }


        // Calculate the total price: 
        // surface + drawers + material + shipping
        public double calculateTotalPrice()
        {
            totalPrice = deskSurfaceAreaPrice + drawerPrice + deskMaterialPrice + rushOrderPrice;

            Console.WriteLine(
                "-----------------------------------------------------" +
                "\nYour desk is ready. The price breakdown is:" +
                "\nDesk size: " + deskArea + "sqin for " + "$" + deskSurfaceAreaPrice +
                "\nDrawer quantity: " + drawerQuantity + " for $" + drawerPrice +
                "\nMaterial: " + deskMaterial + " for $" + deskMaterialPrice +
                "\nRush order: " + rushOrderQuantity + " for $" + rushOrderPrice +
                "\n-----------------------------------------------------" +
                "\nTotal cost: $" + totalPrice);

            return totalPrice;
        }

        // Output results to a file
        private string requestFileOutput ()
        {
            Console.WriteLine("\nWould you like to save this order? Y/N");
            saveOrder = Convert.ToString(Console.ReadLine());

            return saveOrder;
        }

        public void saveFile ()
        {
            if (saveOrder == "Y" || saveOrder == "y")
            {
                string[] orderOutput = { "[",
                                        	"{",
		                                        "\"Size\":" + deskArea + ",",
                                                "\"Size Price\":" + deskSurfaceAreaPrice + ",",
                                                "\"Drawers\":" + drawerQuantity + ",",
                                                "\"Drawer Price\":" + drawerPrice + ",",
                                                "\"Material\":" + deskMaterial + ",",
                                                "\"Material Price\":" + deskMaterialPrice + ",",
                                                "\"Delivery Days\":" + rushOrderQuantity + ",",
                                                "\"Delivery Price\":" + rushOrderPrice + ",",
                                                "\"Total\":" + totalPrice,
                                              "}",
                                            "]" 
                                         };
                System.IO.File.WriteAllLines("SavedOrder.js", orderOutput);

                Console.WriteLine("\nThanks for using the application. Your file has been saved. Press any key to end.");
                Console.ReadLine();
            } else
            {
                Console.WriteLine("\nThanks for using the application. Press any key to end.");
                Console.ReadLine();
            }
        }
    }
}
