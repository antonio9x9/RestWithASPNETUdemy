namespace RestWithASPNETUdemy.Data.Converter.Contract
{
    public interface IParcer<O, D>
    {
        D Parse (O origin);
        List<D> Parse (List<O> origin);
    }
}
