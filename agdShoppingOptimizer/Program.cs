class agdOptimizer
{
    static void Main(string[] args)
    {
        // Display the number of command line arguments.
        //Console.WriteLine("How many items you want to purchase?");
        int itemCount;


        List<double> itemPrices = new List<double> { 2949, 4599, 2699, 2449, 3099, 2499, 1679};
        Console.WriteLine("All items separate price: " + itemPrices.Sum(x => Convert.ToInt32(x)));
        List<List<double>> permutations = GeneratePermutations(itemPrices);

        List<List<double>> bestCart = new List<List<double>>();
        double bestCartPrice = double.MaxValue;
        int number = itemPrices.Count;
        List<string> uniquePartitions = GenerateUniquePartitions(number);
        foreach (var permutation in permutations)
        {
            var carts = GenerateCartsForPermutation(permutation, uniquePartitions);
            foreach (var cart in carts)
            {
                var cartPrice = CalculatePriceForCart(cart);
                if (cartPrice < bestCartPrice)
                {
                    bestCartPrice = cartPrice;
                    bestCart = cart;
                }
            }
        }
        
        Console.WriteLine("Best cart price: " + bestCartPrice);
        Console.WriteLine("Saving: " + (itemPrices.Sum() - bestCartPrice));
        Console.WriteLine("Best cart:");
        foreach (var order in bestCart)
        {
            Console.WriteLine(string.Join(", ", order));
        }
    }

    static double CalculatePriceForCart(List<List<double>> cart)
    {
        double totalPrice = 0;
        foreach (var b in cart)
        {
            List<double> a = new List<double>(b);
            a.Sort();

            if (a.Count == 2)
            {
                a[0] = a[0] * 0.7;
            }
            else if (a.Count == 3)
            {
                a[0] = a[0] * 0.45;
            }
            else if (a.Count == 4)
            {
                a[0] = a[0] * 0.2; 
            }
            else if (a.Count >= 5)
            {
                a[0] = 1;
            }
            totalPrice += a.Sum();
        }
        return totalPrice;
    }


    static List<string> GenerateUniquePartitions(int n, List<int> partition = null, HashSet<string> uniquePartitions = null)
    {
        if (partition == null)
            partition = new List<int>();

        if (uniquePartitions == null)
            uniquePartitions = new HashSet<string>();

        List<string> result = new List<string>();

        if (n == 0)
        {
            partition.Sort();
            // Convert the partition to a string for uniqueness check
            string partitionString = string.Join("", partition);

            // Check if the partition is unique
            if (uniquePartitions.Add(partitionString))
            {
                // Add the unique partition to the result
                result.Add(partitionString);
            }

            return result;
        }

        for (int i = 1; i <= n; i++)
        {
            List<int> newPartition = new List<int>(partition);
            newPartition.Add(i);

            // Recursively generate unique partitions and add them to the result
            result.AddRange(GenerateUniquePartitions(n - i, newPartition, uniquePartitions));
        }

        return result;
    }
    static List<List<List<double>>> GenerateCartsForPermutation(List<double> permutation, List<string> uniquePartitions)
    {
        List<List<List<double>>> allCarts = new List<List<List<double>>>();
        foreach (var partition in uniquePartitions)
        {
            int indexToAdd = 0;
            List<List<double>> cartsForPartition = new List<List<double>>();
            foreach (var letter in partition) {
                Int32.TryParse(letter.ToString(), out int number);
                List<double> cart = new List<double>();
                for (int i = 0; i < number; i++)
                {
                    cart.Add(permutation[indexToAdd]);
                    indexToAdd++;
                }
                cartsForPartition.Add(cart);
            }
            allCarts.Add(cartsForPartition);
        }
        return allCarts;
    }

    static List<List<double>> GeneratePermutations(List<double> numbers)
    {
        List<List<double>> result = new List<List<double>>();
        GeneratePermutationsHelper(numbers, 0, result);
        return result;
    }

    static void GeneratePermutationsHelper(List<double> numbers, int index, List<List<double>> result)
    {
        if (index == numbers.Count - 1)
        {
            result.Add(numbers.ToList());
        }
        else
        {
            for (int i = index; i < numbers.Count; i++)
            {
                Swap(numbers, index, i);
                GeneratePermutationsHelper(numbers, index + 1, result);
                Swap(numbers, index, i); // Backtrack
            }
        }
    }

    static void Swap(List<double> numbers, int i, int j)
    {
        double temp = numbers[i];
        numbers[i] = numbers[j];
        numbers[j] = temp;
    }
}