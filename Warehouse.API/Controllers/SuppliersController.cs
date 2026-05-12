using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;
using Warehouse.Models.DTOs;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService
            _supplierService;

        public SupplierController(
            ISupplierService supplierService
        )
        {
            _supplierService =
                supplierService;
        }

        // =========================
        // LẤY DANH SÁCH NHÀ CUNG CẤP
        // =========================
        [HttpGet]
        [EndpointSummary(
            "Lấy danh sách nhà cung cấp"
        )]

        public IActionResult GetAll()
        {
            var suppliers =
                _supplierService.GetAll();

            return Ok(suppliers);
        }

        // =========================
        // LẤY NHÀ CUNG CẤP THEO ID
        // =========================
        [HttpGet("{id}")]
        [EndpointSummary(
            "Lấy nhà cung cấp theo ID"
        )]

        public IActionResult GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    "ID không hợp lệ"
                );
            }

            var supplier =
                _supplierService.GetById(id);

            if (supplier == null)
            {
                return NotFound(
                    "Không tìm thấy nhà cung cấp"
                );
            }

            return Ok(supplier);
        }

        // =========================
        // THÊM NHÀ CUNG CẤP
        // =========================
        [HttpPost]
        [EndpointSummary(
            "Thêm nhà cung cấp"
        )]

        public IActionResult Add(
            [FromBody]
            CreateSupplierDTO dto
        )
        {
            try
            {
                // VALIDATION MODEL
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState
                    );
                }

                Supplier supplier =
                    new Supplier
                    {
                        SupplierCode =
                            dto.SupplierCode,

                        SupplierName =
                            dto.SupplierName,

                        Phone =
                            dto.Phone,

                        Address =
                            dto.Address
                    };

                _supplierService.Add(
                    supplier
                );

                return Ok(
                    "Thêm nhà cung cấp thành công"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }

        // =========================
        // CẬP NHẬT NHÀ CUNG CẤP
        // =========================
        [HttpPut("{id}")]
        [EndpointSummary(
            "Cập nhật nhà cung cấp"
        )]

        public IActionResult Update(
            int id,
            [FromBody]
            UpdateSupplierDTO dto
        )
        {
            try
            {
                // VALIDATION ID
                if (id <= 0)
                {
                    return BadRequest(
                        "ID không hợp lệ"
                    );
                }

                // VALIDATION MODEL
                if (!ModelState.IsValid)
                {
                    return BadRequest(
                        ModelState
                    );
                }

                var existingSupplier =
                    _supplierService
                    .GetById(id);

                if (existingSupplier == null)
                {
                    return NotFound(
                        "Không tìm thấy nhà cung cấp"
                    );
                }

                Supplier supplier =
                    new Supplier
                    {
                        SupplierId = id,

                        SupplierCode =
                            dto.SupplierCode,

                        SupplierName =
                            dto.SupplierName,

                        Phone =
                            dto.Phone,

                        Address =
                            dto.Address
                    };

                _supplierService.Update(
                    supplier
                );

                return Ok(
                    "Cập nhật nhà cung cấp thành công"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }

        // =========================
        // XÓA NHÀ CUNG CẤP
        // =========================
        [HttpDelete("{id}")]
        [EndpointSummary(
            "Xóa nhà cung cấp"
        )]

        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(
                        "ID không hợp lệ"
                    );
                }

                var supplier =
                    _supplierService
                    .GetById(id);

                if (supplier == null)
                {
                    return NotFound(
                        "Không tìm thấy nhà cung cấp"
                    );
                }

                _supplierService.Delete(id);

                return Ok(
                    "Xóa nhà cung cấp thành công"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }
    }
}