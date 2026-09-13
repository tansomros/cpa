# CPA

Practical Enterprise Framework
built with

ASP.NET Core
Vue 3
PostgreSQL

---

## Technology

* .NET 9
* Vue 3
* PrimeVue
* PostgreSQL
* EF Core
* Dapper
* C#
* Swagger
* EntityFramework    
* Visual Studio 2022

## ขั้นตอนการรันใน visual studio

    ใน solution มีหลาย Project เลือกที่ต้องการรัน

## ขั้นตอนการ build solution บน Terminal

    $ solution> dotnet build -tl

## ขั้นตอนการรัน project ใน Terminal

    $ project> dotnet run

## ขั้นตอนการรัน project ที่เป็น aspnetcore และ auto reload ใน Terminal

    $ project> dotnet watch run

## เครื่องมือสร้างโค้ด Code Scaffolding

### โค้ดสำหรับสร้าง Command
    ```
    $ src\Application\Features> dotnet new ca-usecase --name Create --feature-name Checkups --usecase-type command --return-type int
    ```
### โค้ดสำหรับสร้าง Query
    ```
    $ src\Application> dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
    ```
### หากเกิดปัญหาในการสร้าง code
    If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try again:

    ```
    $> dotnet new install Clean.Architecture.Solution.Template::9.0.10
    ```


## การ Migration Database (Code First) ออกแบบ Entity Class แล้วนำไปสร้างเป็น Database

    1. ไปที่ Tools -> Nuget Package Manager -> Package Manager Console
    2. ใน Tab Package Manager Console เลือก Default Project เป็น Infrastructure
    3. เลือก Project ที่จะรันเป็น API 
    4. RUN คำสั่งต่อไปนี้เพื่อสร้าง Migration Class

    ```
	    PM> Add-Migration InitialCreate -Context CpaDatabaseContext -o Persistence/Migrations
    ```

    5. RUN คำสั่งต่อไปนี้สำหรับนำ Migration Class ที่ได้เอาไป Migrate บนฐานข้อมูล

    ```
	    PM> Update-Database -Context CpaDatabaseContext
    ```

## ในกรณีที่ production แล้ว แต่มีการเปลี่ยนแปลง Domain Entity ต่างๆ ภายหลัง ให้ทำการ Add migration ใหม่โดยตั้งชื่อใหม่ (เปลี่ยนชื่อ InitialCreate เป็นชื่ออื่นตาม) และทำการ Run คำสั่ง Update-Database 

## ในกรณีที่มีการเปลี่ยนแปลง Domain Entity ต่างๆ เพื่อให้ง่ายในการพัฒนา ให้ทำการ Drop Database และลบ Migration Class เดิมออกก่อนดังนี้

    1. RUN คำสั่งต่อไปนี้เพื่อ drop database !!!โปรดระวังการใช้คำสั่งนี้เพราะมันจะ drop database ทิ้งทันทีกลับคืนไม่ได้, โปรดตวรจสอบ Connection string ว่าถูก Database ไหม?
 
    ```
	    PM> Drop-Database -Context CpaDatabaseContext
    ```

    2. RUN คำสั่งต่อไปนี้เพื่อลบ Migration Class ออก

    ```
	    PM> Remove-Migration -Context CpaDatabaseContext
    ```

## การรันทดสอบ

    ```
    $ solution> dotnet test
    ```


## deploy API ก่อนแล้วค่อย build ตัว worker

    ```
    $ solution> bash deploy-api.sh
    $ solution> bash deploy-worker.sh
    ```

## มาตรฐานการออกแบบ API (RESTful API Standards)

เพื่อให้การทำงานของ API เป็นไปตามมาตรฐานสากลและรองรับการสร้างโค้ดอัตโนมัติ (เช่น NSwag) ได้อย่างสมบูรณ์แบบ โปรดปฏิบัติตามกฎดังนี้ในการสร้างหรือแก้ไข Controller:

1. **Routing และ Naming**: ห้ามใส่คำกริยา (Verb) ลงใน Route URL ให้ใช้ HTTP Methods (`GET`, `POST`, `PUT`, `DELETE`) ในการระบุการกระทำแทน
   - ❌ ผิด: `[HttpPost("CreateLab")]`, `[HttpGet("[action]")]`
   - ✅ ถูก: `POST /api/Labs` (เพิ่มข้อมูลใหม่), `GET /api/Labs/visits/1234` (ค้นหาตาม Visit)
2. **การตั้งชื่อ Sub-Resources**: กรณีที่เป็นคำสั่งจำเพาะ ให้ระบุเป็นพาร์ธย่อยในรูปแบบ Noun เช่น `[HttpGet("search")]` หรือ `[HttpPut("batch")]`
3. **การรับ Parameters**:
   - ควบคุมการรับ ID ผ่าน `{id}` ในเส้นทาง URL
   - ข้อมูลการค้นหาทั้งหมด (`SearchTerm`, `StartDate`) ต้องถูกรวมไว้ใน Object และรับผ่าน `[FromQuery]`
   - ข้อมูลการสร้าง/ปรับปรุง (`Create...Command`) ต้องบังคับรับผ่าน `[FromBody]`
4. **Return Types & Swagger**: ใช้ `ActionResult<T>` ทุกครั้งแทนการคืนค่าเป็น `IActionResult` เปล่าๆ เพื่อให้ Swagger สามารถอ่านโครงสร้าง Class ตอน 200 OK ได้อัตโนมัติ
   - ✅ ถูก: `public async Task<ActionResult<BigLionViewModel>> GetBigLion(int id)`
   - ให้ระบุพฤติกรรม Error (`400`, `404`, `500`) ผ่าน `ProducesResponseType` เสมอ เพื่อให้ NSwag สร้าง ApiException ได้อย่างแม่นยำ

## มาตรฐานการร่วมพัฒนา (โปรดปฏิบัติตามอย่างเคร่งครัด)

    1. Interface ให้ขึ้นต้นชื่อไฟล์และชื่อด้วย I ตัวไอพิมพ์ใหญ่ เช่น ICpaDatabaseContext
    2. ให้ตั้งชื่อให้สื่อความหมายและอธิบายตัวเองได้ดี ไม่ว่าจะเป็นชื่อคลาส, ตัวแปร และเมธอด หากไม่แน่ใจให้ปรึกษาทีม
    3. ให้ใช้ PascalCase ในการตั้งชื่อคลาสและเมธอด เช่นคลาส CpaDatabaseContext หรือ เมธอด SaveChangesAsync()
    4. ให้ใช้ PascalCase ในการตั้งชื่อค่าคงที่ Constant ทั้งที่เป็น local constants และ Fields เช่น ConnectionString = "Database"
    5. ให้ใช้ camelCase ในการตั้งชื่อ method arguments, local variables, และ private fields เช่นใน Constructor CpaDatabaseContext(string connectionString)
    6. ถ้าเป็น private instance ให้ใช้ _ underscore นำหน้า เช่น _context
    7. พารามิเตอร์ในฟังก์ชั่นหรือเมธอด หากมีไม่เกิน 3 ตัว ให้เขียนเรียงต่อกัน แต่ถ้าหากเกิน 3 ตัว ให้ขึ้นบรรทัดใหม่ทุกตัว

## ขั้นตอนเข้าร่วมพัฒนา

    1. Clone source code ได้ที่ https://git.suth.go.th/dev/checkup จาก gitlab ผ่าน SourceTree
    2. สร้าง branch ใหม่ที่แตกออกจาก branch หลักเช่น main หรือ branch ที่กำหนด โดยใช้ชื่อ branch ที่สื่อความหมายเช่น FeatureNameDevelopement หรือ ภาษาไทยเช่น พัฒนาฟังก์ชันการทำงานของการเรียกคิว
    3. พัฒนาคุณสมบัติส่วนที่เกี่ยวข้อง
    4. commit การเปลี่ยนแปลง โดยที่หากเป็นการเปลี่ยนแปลงที่เกี่ยวข้องกับหลายไฟล์สามารถรวมกลุ่มเป็น commit เดียวกันได้ แต่ควรแยกส่วนให้ย่อยที่สุดหากทำได้
    5. การตั้งข้อความ commit พยายามให้สื่อความหมายถึงสิ่งที่เปลี่ยนแปลงและชัดเจนและเข้าใจง่ายเป็นภาษาไทย หรือภาษาอังกฤษ
    6. push เข้า branch ที่ remote เพื่อเก็บไว้บน git server (เฉพาะ branch ตัวเองเท่านั้น) **ห้าม push เข้า branch อื่นโดยตรงเด็ดขาด

## ก่อนการรวม source code เข้ากับ Branch หลัก

    1. ต้องมีการเขียน Unit Tests สำหรับ Domain Entity ก่อนเสมอ อย่างน้อยต้องมี test cases ที่ครอบคลุม constructor สำหรับ field ที่ required
    2. สำหรับ Command หรือ Query ต้องมีการเขียน Integration Tests อย่างน้อยครอบคลุม test cases สำหรับการทำงานที่สำเร็จ, การทำงานที่ไม่สำเร็จที่เกิดจากข้อมูลไม่มี, การทำงานที่ไม่สำเร็จที่เกิดจากการส่งข้อมูลมาไม่ตรงกับ Validation Rules หรือคุณสมบัติที่ค่อนข้างวิกฤตและสำคัญ
    3. การไม่เขียน tests สำหรับทดสอบ code ที่ตัวเองเขียนขึ้น เป็นการทำงานที่ไม่มีคุณภาพมีผลกระทบกับผู้ใช้งานอย่างสูง และสร้างความวิตกกังวลให้กับทีมเป็นอย่างมากในตอน Production
    4. การเขียน tests แม้จะไม่ได้การรันตีว่า Production จะไม่มี BUG 100% แต่ก็สามารถป้องกัน error บางอย่างที่ไม่ควรเกิดขึ้นและสามารถตรวจพบในระหว่างพัฒนาได้เลย ดีกว่าไปเจอที่ Production 

## การรวม source code (Pull Request)

    1. commit สิ่งที่เปลี่ยนแปลง และที่ต้องการ stage ให้เรียบร้อย ทั้งที่ modified และ unstaged
    2. หากต้องการรวม source code ที่อยู่ใน bracnh ของตนเองเข้ากับ branch หลักที่แตกออกมา ให้สลับ branch ไปยัง branch ที่ต้องการรวมแล้ว fetch->pull ลงมาหากมี update ก่อนเสมอ
    3. สลับ branch กลับไปยัง branch ตัวเอง แล้วให้คลิกขวาที่ branch ที่ต้องการรวมแล้วเลือก rebase
    4. หากมี conflict ให้ resolve conflict โดยใช้ DiffMerge และปรึกษาทีม
    5. เมื่อ resolve conflict เสร็จแล้วหรือไม่มี conflict ให้ Push ขึ้น branch ตัวเอง
    6. ไปสร้าง Pull Request ใน https://git.suth.go.th/dev/checkup
    7. เลือก branch ต้นทาง และ ปลายทางให้ถูกต้อง
    8. กำหนดผู้รับผิดชอบ (ไม่ต้องกำหนดหากเป็น branch ตัวเอง) และใส่ข้อความอธิบายเกี่ยวกับการขอรวมในครั้งนี้ให้สั้นกระชับเข้าใจง่าย
    9. บันทึกการขอ Merge หาเป็น branch ตัวเอง ก็ merge เองได้เลย
    10. ผู้ที่ถูก assign หรือ branch ปลายทางจะดำเนินการ pull code ลงมา review และ test ก่อนที่จะ accept merge request หรือ reject merge request
    11. หากรวมแล้วเป็นอันเสร็จสิ้น
    12. pull version ใหม่ลงมา แล้วแตก branch ใหม่เพื่อพัฒนาต่อจากนั้น
    13. ทำตามขั้นตอนแรกวนไป

## Architecture Checklist ก่อน Merge ทุก Feature เช่น

 * [] Business Logic อยู่ใน Application หรือไม่
 * [] Repository ไม่มี Business Logic
 * [] Controller ไม่มี Logic
 * [] Validation อยู่ใน FluentValidation
 * [] DTO ไม่รั่วเข้า Domain
 * [] API Response เป็นมาตรฐาน
 * [] Logging ครบ
 * [] Unit Test (ถ้ามี Business Logic สำคัญ)
 * [] ตั้งชื่อตาม Coding Standard