using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.PictureAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class ProductPictureRepository : RepositoryBase<long, ProductPicture> , IProductPictureRepository
{
    private readonly ShopContext _context;
    public ProductPictureRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
    {
        var query = _context.ProductPictures
            .Include(x => x.Product)
            .Select(x => new ProductPictureViewModel()
        {
            Id = x.Id,
            Product = x.Product.Name, // Include
            CreationDate = x.CreationDate.ToString(),
            Picture = x.Picture,
            ProductId = x.ProductId,
            IsRemoved = x.IsRemoved
        });
        if (searchModel.ProductId != 0)
            query = query.Where(x => x.ProductId == searchModel.ProductId);

        return query.OrderByDescending(x => x.Id).ToList();
    }

    public EditProductPicture GetDetails(long id)
    {
        return _context.ProductPictures.Select(x => new EditProductPicture
        {
            Id = x.Id,
            Picture =x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ProductId = x.ProductId
        }).FirstOrDefault(x => x.Id == id);
    }
}