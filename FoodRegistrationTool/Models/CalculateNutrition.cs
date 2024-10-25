public class CalculateNutrition

// All values and logic found from: https://en.wikipedia.org/wiki/Nutri-Score
{
    public static string CalculateScore(string category, int calories, double saturatedFat, double sugar, double salt, double fibre, double protein, int fruitOrVeg)
    {
        int points = 0;

        if (category.ToLower() == "beverage")
        {
            points += CalculateEnergyPointsBeverage(calories);
            points += CalculateSugarPointsBeverage(sugar);
            
            points -= CalculateFruitOrVegetableContentPointsBeverage(fruitOrVeg);
        }
        else // Solid foods
        {
            points += CalculateEnergyPointsSolid(calories);
            points += CalculateSaturatedFatPoints(saturatedFat);
            points += CalculateSugarPointsSolid(sugar);
            points += CalculateSaltPoints(salt);

            points -= CalculateFibrePoints(fibre);
            points -= CalculateProteinPoints(protein);
            points -= CalculateFruitOrVegetableContentPoints(fruitOrVeg);

        }

        return ConvertPointsToScore(points);
    }

    private static int CalculateEnergyPointsBeverage(int calories)
    {
        if (calories <= 0) return 0;
        if (calories <= 30) return 1;
        if (calories <= 60) return 2;
        if (calories <= 90) return 3;
        if (calories <= 120) return 4;
        if (calories <= 150) return 5;
        if (calories <= 180) return 6;
        if (calories <= 210) return 7;
        if (calories <= 240) return 8;
        if (calories <= 270) return 9;
        return 10;
    }

    private static int CalculateEnergyPointsSolid(int calories)
    {
        if (calories < 80) return 0;
        if (calories <= 160) return 1;
        if (calories <= 240) return 2;
        if (calories <= 320) return 3;
        if (calories <= 400) return 4;
        if (calories <= 480) return 5;
        if (calories <= 560) return 6;
        if (calories <= 640) return 7;
        if (calories <= 720) return 8;
        if (calories <= 800) return 9;
        return 10;
    }

    private static int CalculateSaturatedFatPoints(double saturatedFat)
    {
        if (saturatedFat <= 1) return 0;
        if (saturatedFat <= 2) return 1;
        if (saturatedFat <= 3) return 2;
        if (saturatedFat <= 4) return 3;
        if (saturatedFat <= 5) return 4;
        if (saturatedFat <= 6) return 5;
        if (saturatedFat <= 7) return 6;
        if (saturatedFat <= 8) return 7;
        if (saturatedFat <= 9) return 8;
        if (saturatedFat <= 10) return 9;
        return 10;
    }

    private static int CalculateSugarPointsBeverage(double sugar)
    {
        if (sugar <= 0) return 0;
        if (sugar < 1.5) return 1;
        if (sugar < 3) return 2;
        if (sugar < 4.5) return 3;
        if (sugar < 6) return 4;
        if (sugar < 7.5) return 5;
        if (sugar < 9) return 6;
        if (sugar < 10.5) return 7;
        if (sugar < 12) return 8;
        if (sugar < 13.5) return 9;
        return 10;
    }

    private static int CalculateSugarPointsSolid(double sugar)
    {
        if (sugar <= 4.5) return 0;
        if (sugar <= 9) return 1;
        if (sugar <= 13.5) return 2;
        if (sugar <= 18) return 3;
        if (sugar <= 22.5) return 4;
        if (sugar <= 27) return 5;
        if (sugar <= 31) return 6;
        if (sugar <= 36) return 7;
        if (sugar <= 40) return 8;
        if (sugar <= 45) return 9;
        return 10;
    }

    private static int CalculateSaltPoints(double salt)
    {
        if (salt < 90) return 0;
        if (salt <= 180) return 1;
        if (salt <= 270) return 2;
        if (salt <= 360) return 3;
        if (salt <= 450) return 4;
        if (salt <= 540) return 5;
        if (salt <= 630) return 6;
        if (salt <= 720) return 7;
        if (salt <= 810) return 8;
        if (salt <= 900) return 9;
        return 10;
    }

    private static int CalculateFibrePoints(double fibre)
    {
        if (fibre <= 0.7) return 0;
        if (fibre <= 1.4) return 1;
        if (fibre <= 2.1) return 2;
        if (fibre <= 2.8) return 3;
        if (fibre <= 3.5) return 4;
        return 5;
    }

    private static int CalculateProteinPoints(double protein)
    {
        if (protein <= 1.6) return 0;
        if (protein <= 3.2) return 1;
        if (protein <= 4.8) return 2;
        if (protein <= 6.4) return 3;
        if (protein <= 8) return 4;
        return 5;
    }

    private static int CalculateFruitOrVegetableContentPoints(int fruitOrVeg)
    {
        if (fruitOrVeg <= 40) return 0;
        if (fruitOrVeg <= 60) return 1;
        if (fruitOrVeg <= 80) return 2;
        return 5;
    }

    private static int CalculateFruitOrVegetableContentPointsBeverage(int fruitOrVeg)
    {
        if (fruitOrVeg <= 40) return 0;
        if (fruitOrVeg <= 60) return 2;
        if (fruitOrVeg <= 80) return 4;
        return 10;
    }



    private static string ConvertPointsToScore(int points)
    {
        // If beverage is selected and points are over 0
        if (CalculateEnergyPointsBeverage(points) + CalculateSugarPointsBeverage(points) > 0)
        {
            if (points <= 2) return "B";
            if (points <= 10) return "C";
            if (points <= 18) return "D";
            if (points <= 40) return "E";
            return "Something went wrong";
        }
        else
        {
            if (points <= -1) return "A";
            if (points <= 2) return "B";
            if (points <= 10) return "C";
            if (points <= 18) return "D";
            if (points <= 40) return "E";
            return "Something went wrong";
        }
        
    }

}



