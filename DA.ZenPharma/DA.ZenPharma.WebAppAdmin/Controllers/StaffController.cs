using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.BranchDto;
using DA.ZenPharma.Application.Dtos.RoleDtos;
using DA.ZenPharma.Application.Dtos.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DA.ZenPharma.WebAppAdmin.Controllers
{
    public class StaffController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7034/api/User";
        private readonly Guid StaffRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        public StaffController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<UserDto>>(
                $"{ApiUrl}/paged?roleId={StaffRoleId}&page={page}&pageSize={pageSize}");

            if (response == null)
            {
                return View("Error");
            }

            ViewBag.TotalItems = response.TotalItems;
            ViewBag.PageSize = response.PageSize;
            ViewBag.TotalPages = response.TotalPages;
            ViewBag.Page = page;

            return View(response);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var staff = await _httpClient.GetFromJsonAsync<UserDto>($"{ApiUrl}/{id}");

            if (staff == null || staff.RoleId != StaffRoleId)
                return NotFound("Không tìm thấy nhân viên.");

            return View(staff);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await _httpClient.GetFromJsonAsync<List<RoleDto>>("https://localhost:7034/api/Role");
            roles = roles?.Where(r =>
                            !string.Equals(r.RoleName, "Customer", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

            // Lấy danh sách chi nhánh từ API (ví dụ API chi nhánh)
            var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
            ViewBag.Branches = new SelectList(branches, "Id", "BranchName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var roles = await _httpClient.GetFromJsonAsync<List<RoleDto>>("https://localhost:7034/api/Role");
                roles = roles?.Where(r =>
                                !string.Equals(r.RoleName, "Customer", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase)).ToList();
                ViewBag.Roles = new SelectList(roles, "Id", "RoleName");

                var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                ViewBag.Branches = new SelectList(branches, "Id", "BranchName");

                return View(dto);
            }

            dto.CreatedAt = DateTime.UtcNow;

            var response = await _httpClient.PostAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm nhân viên thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi thêm nhân viên.";

            var rolesRetry = await _httpClient.GetFromJsonAsync<List<RoleDto>>("https://localhost:7034/api/Role");
            rolesRetry = rolesRetry?.Where(r =>
                            !string.Equals(r.RoleName, "Customer", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.Roles = new SelectList(rolesRetry, "Id", "RoleName");

            var branchesRetry = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
            ViewBag.Branches = new SelectList(branchesRetry, "Id", "BranchName");

            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var staff = await _httpClient.GetFromJsonAsync<UserUpdateDto>($"{ApiUrl}/{id}");

            if (staff == null || staff.RoleId != StaffRoleId)
                return NotFound("Không tìm thấy nhân viên.");

            var roles = await _httpClient.GetFromJsonAsync<List<RoleDto>>("https://localhost:7034/api/Role");
            roles = roles?.Where(r =>
                            !string.Equals(r.RoleName, "Customer", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase)).ToList();
            ViewBag.Roles = new SelectList(roles, "Id", "RoleName", staff.RoleId);

            // Lấy danh sách chi nhánh
            var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
            ViewBag.Branches = new SelectList(branches, "Id", "BranchName", staff.BranchId);

            return View(staff);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var roles = await _httpClient.GetFromJsonAsync<List<RoleDto>>("https://localhost:7034/api/Role");
                roles = roles?.Where(r =>
                                !string.Equals(r.RoleName, "Customer", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase))
                              .ToList();
                ViewBag.Roles = new SelectList(roles, "Id", "RoleName", dto.RoleId);

                var branches = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
                ViewBag.Branches = new SelectList(branches, "Id", "BranchName", dto.BranchId);

                return View(dto);
            }

            dto.RoleId = StaffRoleId;

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật nhân viên thành công!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đã có lỗi xảy ra khi cập nhật nhân viên.";

            var rolesAgain = await _httpClient.GetFromJsonAsync<List<RoleDto>>("https://localhost:7034/api/Role");
            rolesAgain = rolesAgain?.Where(r =>
                            !string.Equals(r.RoleName, "Customer", StringComparison.OrdinalIgnoreCase) &&
                            !string.Equals(r.RoleName, "Admin", StringComparison.OrdinalIgnoreCase))
                          .ToList();
            ViewBag.Roles = new SelectList(rolesAgain, "Id", "RoleName", dto.RoleId);

            var branchesAgain = await _httpClient.GetFromJsonAsync<List<BranchDto>>("https://localhost:7034/api/Branch");
            ViewBag.Branches = new SelectList(branchesAgain, "Id", "BranchName", dto.BranchId);

            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _httpClient.GetFromJsonAsync<UserDto>($"{ApiUrl}/{id}");
            if (user == null || user.RoleId != StaffRoleId)
                return NotFound("Không tìm thấy nhân viên.");

            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Đã xóa nhân viên thành công!";
            else
                TempData["ErrorMessage"] = "Đã có lỗi khi xóa nhân viên!";

            return RedirectToAction("Index");
        }
    }
}
