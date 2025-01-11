$restaurantNames = @(
    "KFC", "McDonald's", "Burger King", "Subway", "Pizza Hut", "Domino's Pizza", "Taco Bell", "Wendy's",
    "Chick-fil-A", "Popeyes", "Dunkin' Donuts", "Starbucks", "Panera Bread", "Chipotle", "Five Guys",
    "In-N-Out Burger", "Shake Shack", "Arby's", "Olive Garden", "Red Lobster", "Cheesecake Factory",
    "Buffalo Wild Wings", "Applebee's", "Chili's", "Outback Steakhouse", "Texas Roadhouse", "IHOP",
    "Denny's", "Cracker Barrel", "TGI Friday's", "Ruby Tuesday", "P.F. Chang's", "Bonefish Grill",
    "Carrabba's", "Maggiano's", "BJ's Restaurant", "Yard House", "Seasons 52", "Fleming's", "Ruth's Chris",
    "Morton's", "The Capital Grille", "LongHorn Steakhouse", "The Melting Pot", "Benihana", "Nobu",
    "Sushi Samba", "Gordon Ramsay Steak", "Hell's Kitchen", "Bubba Gump Shrimp Co.", "Rainforest Cafe",
    "Hard Rock Cafe", "Planet Hollywood", "Joe's Crab Shack", "Hooters", "Wingstop", "Zaxby's", "Raising Cane's",
    "El Pollo Loco", "Pollo Tropical", "Boston Market", "Qdoba", "Moe's Southwest Grill", "Del Taco",
    "Jack in the Box", "Carl's Jr.", "Hardee's", "White Castle", "Whataburger", "Culver's", "Freddy's",
    "Steak 'n Shake", "Checkers", "Rally's", "Sonic Drive-In", "A&W", "Long John Silver's", "Bojangles",
    "Church's Chicken", "WingStreet", "Papa John's", "Little Caesars", "Marco's Pizza", "Blaze Pizza",
    "MOD Pizza", "Pieology", "California Pizza Kitchen", "Round Table Pizza", "Sbarro", "Cici's Pizza",
    "Papa Murphy's", "Hungry Howie's", "Jet's Pizza", "Mellow Mushroom", "Mountain Mike's Pizza",
    "Pizza Ranch", "Rosati's Pizza", "Lou Malnati's", "Giordano's", "Uno Pizzeria & Grill", "Old Chicago",
    "Bertucci's", "East of Chicago Pizza", "Fox's Pizza Den", "Gatti's Pizza", "Godfather's Pizza",
    "Happy Joe's", "Imo's Pizza", "LaRosa's Pizzeria", "Ledo Pizza", "Monical's Pizza", "Pizza Inn",
    "Shakey's Pizza", "Snappy Tomato Pizza", "Toppers Pizza", "Village Inn Pizza", "Zpizza", "Nando's",
    "Pret A Manger", "Wagamama", "Yo! Sushi", "Itsu", "Leon", "Giraffe", "Bill's", "Zizzi", "Prezzo",
    "Bella Italia", "Cafe Rouge", "Las Iguanas", "TGI Fridays", "Frankie & Benny's", "Chiquito", "Brewers Fayre",
    "Beefeater", "Harvester", "Toby Carvery", "Miller & Carter", "All Bar One", "Slug & Lettuce", "Revolution",
    "Walkabout", "O'Neill's", "Greene King", "Marston's", "Wetherspoon", "Mitchells & Butlers", "Fuller's",
    "Young's", "Stonegate", "Loungers", "Cosy Club", "The Ivy", "Dishoom", "Hakkasan", "The Wolseley",
    "Sketch", "Duck & Waffle", "The Ledbury", "The Clove Club", "Gymkhana", "Dabbous", "Pollen Street Social",
    "Lyle's", "The Palomar", "Barrafina", "Brat", "Kiln", "Smoking Goat", "Padella", "Flat Iron", "Blacklock",
    "Hawksmoor", "Goodman", "Burger & Lobster", "Honest Burgers", "Patty & Bun", "MeatLiquor", "Byron",
    "GBK", "Shake Shack", "Five Guys", "Smashburger", "Wimpy", "Ed's Easy Diner", "Gourmet Burger Kitchen",
    "The Breakfast Club", "Bill's", "The Diner", "Balans", "Caravan", "Granger & Co", "The Wolseley",
    "The Ivy", "Dishoom", "Hakkasan", "The Ledbury", "The Clove Club", "Gymkhana", "Dabbous", "Pollen Street Social",
    "Lyle's", "The Palomar", "Barrafina", "Brat", "Kiln", "Smoking Goat", "Padella", "Flat Iron", "Blacklock",
    "Hawksmoor", "Goodman", "Burger & Lobster", "Honest Burgers", "Patty & Bun", "MeatLiquor", "Byron",
    "GBK", "Shake Shack", "Five Guys", "Smashburger", "Wimpy", "Ed's Easy Diner", "Gourmet Burger Kitchen",
    "The Breakfast Club", "Bill's", "The Diner", "Balans", "Caravan", "Granger & Co"
)

$menuItems = @(
    @{ Name = "Coffee"; Price = 10.00; Description = "Coffee with milk"; KiloCalories = 100 },
    @{ Name = "Chicken"; Price = 20.00; Description = "Fried chicken"; KiloCalories = 300 },
    @{ Name = "Big Mac"; Price = 30.00; Description = "Big Mac with fries"; KiloCalories = 300 },
    @{ Name = "Ice Cream"; Price = 9.00; Description = "Vanilla ice cream"; KiloCalories = 200 },
    @{ Name = "Whopper"; Price = 25.00; Description = "Whopper with cheese"; KiloCalories = 350 },
    @{ Name = "Fries"; Price = 5.00; Description = "French fries"; KiloCalories = 200 },
    @{ Name = "Sub Sandwich"; Price = 15.00; Description = "Sub sandwich with ham and cheese"; KiloCalories = 250 },
    @{ Name = "Cookies"; Price = 3.00; Description = "Chocolate chip cookies"; KiloCalories = 150 },
    @{ Name = "Pepperoni Pizza"; Price = 20.00; Description = "Pepperoni pizza with extra cheese"; KiloCalories = 400 },
    @{ Name = "Garlic Bread"; Price = 7.00; Description = "Garlic bread with cheese"; KiloCalories = 200 },
    @{ Name = "Margherita Pizza"; Price = 18.00; Description = "Margherita pizza with fresh tomatoes"; KiloCalories = 350 },
    @{ Name = "Chicken Wings"; Price = 12.00; Description = "Spicy chicken wings"; KiloCalories = 250 },
    @{ Name = "Taco"; Price = 8.00; Description = "Beef taco with lettuce and cheese"; KiloCalories = 150 },
    @{ Name = "Burrito"; Price = 10.00; Description = "Chicken burrito with beans"; KiloCalories = 300 },
    @{ Name = "Baconator"; Price = 22.00; Description = "Baconator with extra bacon"; KiloCalories = 450 },
    @{ Name = "Frosty"; Price = 6.00; Description = "Chocolate frosty"; KiloCalories = 200 },
    @{ Name = "Chicken Sandwich"; Price = 15.00; Description = "Chicken sandwich with pickles"; KiloCalories = 300 },
    @{ Name = "Waffle Fries"; Price = 5.00; Description = "Waffle fries with ketchup"; KiloCalories = 200 },
    @{ Name = "Chicken Tenders"; Price = 12.00; Description = "Spicy chicken tenders"; KiloCalories = 250 },
    @{ Name = "Biscuits"; Price = 4.00; Description = "Buttermilk biscuits"; KiloCalories = 150 },
    @{ Name = "Glazed Donut"; Price = 3.00; Description = "Glazed donut with sprinkles"; KiloCalories = 200 },
    @{ Name = "Latte"; Price = 5.00; Description = "Latte with almond milk"; KiloCalories = 100 },
    @{ Name = "Cappuccino"; Price = 6.00; Description = "Cappuccino with extra foam"; KiloCalories = 150 },
    @{ Name = "Blueberry Muffin"; Price = 4.00; Description = "Blueberry muffin with crumble topping"; KiloCalories = 250 },
    @{ Name = "Broccoli Cheddar Soup"; Price = 8.00; Description = "Broccoli cheddar soup in a bread bowl"; KiloCalories = 300 },
    @{ Name = "Caesar Salad"; Price = 10.00; Description = "Caesar salad with grilled chicken"; KiloCalories = 200 },
    @{ Name = "Burrito Bowl"; Price = 12.00; Description = "Burrito bowl with chicken and rice"; KiloCalories = 400 },
    @{ Name = "Chips and Guacamole"; Price = 6.00; Description = "Tortilla chips with guacamole"; KiloCalories = 250 },
    @{ Name = "Cheeseburger"; Price = 15.00; Description = "Cheeseburger with all the toppings"; KiloCalories = 500 },
    @{ Name = "Milkshake"; Price = 7.00; Description = "Vanilla milkshake with whipped cream"; KiloCalories = 300 },
    @{ Name = "Double-Double"; Price = 14.00; Description = "Double-double burger with cheese"; KiloCalories = 450 },
    @{ Name = "Animal Style Fries"; Price = 8.00; Description = "Fries with cheese, grilled onions, and special sauce"; KiloCalories = 350 },
    @{ Name = "ShackBurger"; Price = 16.00; Description = "ShackBurger with lettuce, tomato, and ShackSauce"; KiloCalories = 400 },
    @{ Name = "Crinkle-Cut Fries"; Price = 6.00; Description = "Crinkle-cut fries with cheese sauce"; KiloCalories = 300 },
    @{ Name = "Roast Beef Sandwich"; Price = 12.00; Description = "Roast beef sandwich with horsey sauce"; KiloCalories = 350 },
    @{ Name = "Curly Fries"; Price = 5.00; Description = "Curly fries with Arby's sauce"; KiloCalories = 250 }
)

$categories = @("Fast Food", "Chinese", "Indian", "Italian", "Mexican", "Japanese", "Mediterranean", "Burgers", "Bakery", "Cafe")

function Generate-Restaurant {
    param (
        [int]$index
    )
    $name = $restaurantNames | Get-Random
    $category = $categories | Get-Random
    $menuItemsSample = $menuItems | Get-Random -Count 2
    return @{
        Name = "$name"
        Description = "$name is a popular restaurant."
        Category = $category
        HasDelivery = $true
        Email = "$($name -replace ' ', '')$index@test.com"
        ContactNumber = "$(Get-Random -Minimum 1000000000 -Maximum 9999999999)"
        Address = @{
            Street = "Street $index"
            City = "City $index"
            County = "County $index"
            PostCode = "M4 7W$($index % 10)"
            Country = "UK"
        }
        MenuItems = $menuItemsSample
        OwnerId = 1
    }
}

$restaurants = @()
for ($i = 1; $i -le 5000; $i++) {
    $restaurants += Generate-Restaurant -index $i
}

$restaurants | ConvertTo-Json -Depth 3 | Set-Content -Path "restaurants.json"