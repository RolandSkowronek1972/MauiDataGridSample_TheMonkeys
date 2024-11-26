namespace ToCIVP_II
{
    public class TheProducts
    {
        private TheProducts product;

        public int Id { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int CategryID { get; set; }
        public string SubCategory { get; set; }
        public int SubCategoryID { get; set; }
        public string Image { get; set; }
        public float Carbohydrates { get; set; }
        public float Calories { get; set; }
    }
}
