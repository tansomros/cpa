## Coding Standard & Style Guide

## 1. บทนำ (Introduction)

* วัตถุประสงค์: เพื่อสร้างมาตรฐานการเขียนโค้ดภาษา C# ให้มีคุณภาพ อ่านง่าย และลดข้อผิดพลาดในทีม
* ขอบเขตการใช้งาน: ใช้กับโปรเจกต์ .NET / C# ทั้งหมดภายในองค์กร

## 2. กฎการตั้งชื่อ (Naming Conventions)
การตั้งชื่อใน C# จะอ้างอิงตาม Microsoft Guidelines เป็นหลัก:

* PascalCase: ใช้สำหรับ Class, Record, Struct, Interface, Method, Property, Namespace และ Public Field
* ตัวอย่าง: public class CustomerService, public void CalculateTotal()
   * หมายเหตุ: Interface ต้องขึ้นต้นด้วยตัวอักษร I เสมอ (เช่น ICustomerRepository)
* camelCase: ใช้สำหรับ Local Variable, Method Parameter และ Private/Internal Field
* ตัวอย่าง: int totalItems = 0;, public void Save(string userName)
* camelCase พร้อมเครื่องหมายขีดล่าง _: ใช้สำหรับ Private/Protected Readonly Field เพื่อแยกแยะจากตัวแปรทั่วไป
* ตัวอย่าง: private readonly ICustomerRepository _customerRepository;
* ตัวพิมพ์ใหญ่ทั้งหมด (UPPERCASE): ใช้สำหรับค่าคงที่ (const)
* ตัวอย่าง: public const int MAX_RETRIES = 3;

## 3. รูปแบบและการจัดวางโค้ด (Layout & Formatting)

* Indentation: ใช้ Space 4 ช่อง (ห้ามใช้ Tab)
* Braces {}: ใช้ระบบ Allman style (วงเล็บปีกกาเปิดและปิดต้องอยู่แยกบรรดัดคนละบรรทัด)

if (isReady)
{
    DoSomething();
}

* Line Breakers: จำกัดความยาวโค้ดไม่เกิน 120 ตัวอักษรต่อบรรทัด
* Implicit Typing (var): ให้ใช้ var ก็ต่อเมื่อประเภทของข้อมูลนั้นชัดเจนจากฝั่งขวามือของคำสั่งเท่านั้น
* ควรใช้: var users = new List<User>();
   * ไม่ควรใช้: var data = GetData(); (เนื่องจากอ่านแล้วไม่รู้ทันทีว่าเป็น Data Type อะไร)

## 4. แนวทางการเขียนโค้ด (Coding Practices)

* Async/Await: เมธอดที่เป็น Asynchronous ทั้งหมดต้องลงท้ายด้วยคำว่า Async เสมอ และต้องส่งต่อ CancellationToken เพื่อรองรับการยกเลิกทำงาน
* ตัวอย่าง: public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken)
* String Interpolation: ใช้ $"" แทนการต่อสตริงด้วยเครื่องหมาย + หรือ String.Format() เพื่อให้อ่านง่าย
* ตัวอย่าง: string message = $"Hello, {user.Name}";
* LINQ (Language Integrated Query): ให้ใช้รูปแบบ Method Syntax เป็นหลักเพื่อความกระชับ เว้นแต่คิวรีมีความซับซ้อนมากจึงใช้ Query Syntax
* ตัวอย่าง: var activeUsers = users.Where(u => u.IsActive).ToList();
* Null Safety: เปิดใช้งานฟีเจอร์ Nullable Reference Types ในไฟล์ .csproj เพื่อป้องกัน NullReferenceException

## 5. การจัดการข้อผิดพลาดและบันทึกข้อมูล (Exception Handling & Logging)

* ดักจับเจาะจงประเภท: หลีกเลี่ยงการ catch (Exception ex) แบบเหวี่ยงแห ให้ดักจับ Exception ที่เจาะจงก่อนเสมอ
* ห้ามกลืน Error: ห้ามปล่อยบล็อก catch ว่างเปล่าโดยไม่มีการทำงานใดๆ
* การ Rethrow: หากต้องการโยน Exception ต่อ ให้ใช้คำสั่ง throw; เสมอ ห้ามใช้ throw ex; เพราะจะทำให้ข้อมูล Stack Trace เดิมสูญหาย
* Logging: ใช้ Dependency Injection ส่ง ILogger<T> เข้ามาบันทึกเหตุการณ์สำคัญ (Information, Warning, Error)

## 6. การเขียนโครงสร้างคลาส (Class Structure)
จัดลำดับองค์ประกอบภายใน Class จากบนลงล่างดังนี้:

   1. Private / Protected Fields (_variables)
   2. Constructors
   3. Public Properties
   4. Public Methods
   5. Private / Internal Methods

## 7. เครื่องมือตรวจสอบอัตโนมัติ (Automation Tools)
เพื่อลดภาระการรีวิวโค้ด ให้ทีมติดตั้งและตั้งค่าเครื่องมือเหล่านี้:

* EditorConfig: ใช้ไฟล์ .editorconfig เพื่อล็อกฟอร์แมตโค้ดให้ตรงกันทั้งทีมบน Visual Studio / VS Code
* Roslyn Analyzers: เปิดใช้งานการตรวจจับ Warning และ Error ตั้งแต่ตอน Code Compile


นี่คือการตั้งค่าที่เพิ่มกฎเฉพาะสำหรับ Entity Framework Core (EF Core) และ LINQ เพื่อป้องกันปัญหาประสิทธิภาพ (Performance) เช่น N+1 Query และการดึงข้อมูลเกินความจำเป็น (Over-fetching) ครับ
คุณสามารถนำโค้ดด้านล่างนี้ไป แปะต่อท้าย ไฟล์ .editorconfig เดิมที่สร้างไว้ได้เลยครับ:

#### 5. กฎสำหรับ Entity Framework Core และ LINQ ####

# บังคับให้ใช้ LINQ Method Syntax เป็นหลักเพื่อความเป็นระเบียบ (เช่น .Where().ToList())
dotnet_style_prefer_collection_expression = true:warning

# ป้องกันปัญหา N+1 Query: บังคับให้ใช้ .Include() แทนการปล่อยให้ Lazy Loading ทำงานแบบเงียบๆ
# (เปิดการแจ้งเตือนจาก Roslyn Analyzer สำหรับ EF Core)
dotnet_diagnostic.EFG001.severity = warning

# บังคับใช้ AsNoTracking() เมื่อดึงข้อมูลมาอ่านอย่างเดียว (Read-Only) เพื่อประหยัดหน่วยความจำ
dotnet_diagnostic.EFG002.severity = warning

# ห้ามใช้ .ToList() หรือ .ToArray() ตรงกลาง Query ก่อนสั่งคำสั่งกรองข้อมูล
# เพื่อป้องกันการดึงข้อมูลทั้งหมดจาก Database มากรองบน Memory (Client-Side Evaluation)
dotnet_diagnostic.EF1001.severity = error

# แจ้งเตือนเมื่อมีการใช้ Query ที่ส่งผลให้เกิดการทำงานที่ไม่มีประสิทธิภาพบน Database Server
dotnet_diagnostic.EF2100.severity = warning

------------------------------
## แนวทางปฏิบัติสำหรับทีม (Team Guide Add-on)
เนื่องจากไฟล์ .editorconfig ไม่สามารถตรวจจับตรรกะระดับลึกได้ทั้งหมด จึงแนะนำให้เพิ่มกฎ 3 ข้อนี้เข้าไปใน เอกสารคู่มือหลัก (Coding Standard Document) ในหมวดหมู่ EF Core ด้วยครับ:
## 1. การดึงข้อมูลเท่าที่จำเป็น (Avoid Projection Pitfalls)

* กฎ: ห้ามดึงข้อมูลทั้ง Entity หากต้องการใช้เพียงไม่กี่คอลัมน์ ให้ใช้ .Select() เพื่อดึงเฉพาะข้อมูลที่ระบุเสมอ
* ไม่ควรทำ: var users = _context.Users.ToList(); // ดึงมาทุกคอลัมน์รวมถึง PasswordHash
* ควรทำ: var users = _context.Users.Select(u => new UserDto { Id = u.Id, Name = u.Name }).ToList();

## 2. การแบ่งหน้าข้อมูล (Mandatory Pagination)

* กฎ: การดึงข้อมูลประเภทตารางรายการ (List) ที่เชื่อมต่อกับ Database ห้ามใช้ .ToList() เปล่าๆ โดยไม่มีการจำกัดจำนวน ต้องใช้ .Skip() และ .Take() เพื่อทำ Pagination เสมอ

## 3. ป้องกันปัญหาโครงสร้างตาราง (Database Schema Safety)

* กฎ: ทุก Entity ที่เป็น Class สำหรับสร้างตาราง ต้องกำหนดประเภทข้อมูลและขนาดสูงสุด ([StringLength] หรือ .HasMaxLength()) ให้ชัดเจน ห้ามปล่อยให้เกิดประเภทข้อมูล nvarchar(max) โดยไม่จำเป็น

------------------------------