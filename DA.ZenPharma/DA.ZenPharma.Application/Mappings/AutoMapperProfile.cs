using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.AddressDto;
using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Dtos.CartDto;
using DA.ZenPharma.Application.Dtos.CategoryDto;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDtos;
using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;
using DA.ZenPharma.Application.Dtos.OrderDetailDtos;
using DA.ZenPharma.Application.Dtos.OrderDto;
using DA.ZenPharma.Application.Dtos.PrescriptionDto;
using DA.ZenPharma.Application.Dtos.ProductDto;
using DA.ZenPharma.Application.Dtos.RoleDtos;
using DA.ZenPharma.Application.Dtos.SupplierDto;
using DA.ZenPharma.Application.Dtos.UserDto;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Domain.Enums;
using DA.ZenPharma.WebAppAdmin.Helpers;
using static Azure.Core.HttpHeader;

namespace DA.ZenPharma.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

            // Product
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Product, ProductSearchDto>()
                .ForMember(dest => dest.UnitDisplayName,
                           opt => opt.MapFrom(src =>
                               Enum.IsDefined(typeof(UnitType), src.Unit)
                                   ? EnumHelper.GetDisplayName(src.Unit)
                                   : "Không xác định"));


            // Category
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

            // Supplier
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Supplier, SupplierCreateDto>().ReverseMap();
            CreateMap<Supplier, SupplierUpdateDto>().ReverseMap();
            CreateMap<Supplier, SupplierSearchDto>().ReverseMap();

            // Branch
            CreateMap<Branch, BranchDto>().ReverseMap();
            CreateMap<Branch, BranchCreateDto>().ReverseMap();
            CreateMap<Branch, BranchUpdateDto>().ReverseMap();

            // Import Invoice
            CreateMap<ImportInvoice, ImportInvoiceDto>()
                .ForMember(dest => dest.SupplierName,
                    opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : ""))
                .ForMember(dest => dest.HandledByStaffName,
                    opt => opt.MapFrom(src => src.Staff != null
                        ? src.Staff.FirstName + " " + src.Staff.LastName
                        : ""))
                .ForMember(dest => dest.BranchName,
                    opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : ""));

            CreateMap<ImportInvoiceCreateDto, ImportInvoice>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Details, opt => opt.Ignore());
            CreateMap<ImportInvoice, ImportInvoiceUpdateDto>().ReverseMap();


            // Import Invoice Detail
            CreateMap<ImportInvoiceDetailCreateDto, ImportInvoiceDetail>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => (src.Quantity ?? 0) * (src.UnitPrice ?? 0)));

            // ImportInvoiceDetail -> ImportInvoiceDetailDto
            CreateMap<ImportInvoiceDetail, ImportInvoiceDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.InventoryBatches, opt => opt.MapFrom(src => src.InventoryBatches));


            // InventoryBatch
            CreateMap<InventoryBatch, InventoryBatchDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

            CreateMap<InventoryBatchUpdateDto, InventoryBatch>();
            CreateMap<InventoryBatchCreateDto, InventoryBatch>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            // Order
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderUpdateDto>().ReverseMap();
            CreateMap<OrderCreateDto, Order>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.Details));

            //Order Detail
            CreateMap<OrderDetailCreateDto, OrderDetail>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null)); ;
            CreateMap<OrderDetailUpdateDto, OrderDetail>().ReverseMap();

            // Prescription
            CreateMap<Prescription, PrescriptionDto>().ReverseMap();
            CreateMap<Prescription, PrescriptionCreateDto>().ReverseMap();
            CreateMap<Prescription, PrescriptionUpdateDto>().ReverseMap();

            // Cart
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<Cart, CartCreateDto>().ReverseMap();
            CreateMap<Cart, CartUpdateDto>().ReverseMap();

            // Address
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Address, AddressCreateDto>().ReverseMap();
            CreateMap<Address, AddressUpdateDto>().ReverseMap();

            //Role
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>().ReverseMap();
        }
    }
}
