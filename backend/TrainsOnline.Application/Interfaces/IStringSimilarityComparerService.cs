namespace TrainsOnline.Application.Interfaces
{
    public interface IStringSimilarityComparerService
    {
        bool AreSimilar(string s0, string s1, double threshold = 0.6);
        bool AreNotSimilar(string s0, string s1, double threshold = 0.6);
    }
}