using System.Xml.Linq;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private IProductRepository _productRepository;

    public ProductApplication(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        
    }

    public OperationResult Create(CreateProduct command)
    {
        var operation = new OperationResult();
        if (_productRepository.Exists(x => x.Name == command.Name))
            return operation.Failed(ApplicationMessages.Dulpicated);
        else
        {
            var slug = command.Slug.Slugify();
            var product = new Product(command.Name, command.Code, command.UnitPrice, command.ShortDescription,
                command.Description, command.Picture, command.PictureAlt, command.PictureTitle, command.CategoryId,slug
                ,command.Keyworks, command.MetaDescription);
            _productRepository.Create(product);
            _productRepository.SaveChanges();
        }

        return operation.Succedded();
    }

    public OperationResult Edit(EditProduct command)
    {
        var operation = new OperationResult();
        var product = _productRepository.Get(command.Id);
        if (product == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);
        if (_productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = command.Slug.Slugify();
        product.Edit(command.Name, command.Code, command.UnitPrice, command.ShortDescription,
            command.Description, command.Picture, command.PictureAlt, command.PictureTitle, command.CategoryId,slug
            ,command.Keyworks, command.MetaDescription);
        _productRepository.SaveChanges();
        return operation.Succedded();


    }

    public OperationResult IsStock(long id)
    {
        var operation = new OperationResult();
        var product = _productRepository.Get(id);
        if (product == null)
            operation.Failed(ApplicationMessages.RecordNotFound);
        product.IsStock();
        _productRepository.SaveChanges();
        return operation.Succedded();
    }

    public OperationResult NotInStock(long id)
    {
        var operation = new OperationResult();
        var product = _productRepository.Get(id);
        if (product == null)
            operation.Failed(ApplicationMessages.RecordNotFound);
        product.NotInStock();
        _productRepository.SaveChanges();
        return operation.Succedded();
    }

    public EditProduct GetDetails(long id)
    {
        return _productRepository.GetDetails(id);
    }

    public List<ProductViewModel> Search(ProductSearchModel searchModel)
    {
        return _productRepository.Search(searchModel);
    }

    public List<ProductViewModel> GetProducts()
    {
        return _productRepository.GetProducts();
    }
}