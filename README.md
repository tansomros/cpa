# CPA

Practical Enterprise Framework
built with

ASP.NET Core
Vue 3
PostgreSQL

---

## Technology

* .NET 10
* C#
* ASP.NET Core
* EF Core + Npgsql
* Dapper (ยังไม่ได้ติดตั้ง จะเพิ่มเข้ามาตาม [ADR-006](docs/adr/ADR-006-orm-strategy.md) เมื่อเริ่มทำงานค้นหา รายงาน และ dashboard)
* PostgreSQL
* Swagger
* Vue 3
* Vuetify 3 (template Vuexy)
* Visual Studio (รุ่นที่รองรับ .NET 10)

## เอกสารประกอบ

* [Coding Standard](docs/coding-standard.md)
* [Database Convention](docs/database.md)
* [กติกาการจัดการวันที่](docs/date-convention.md)
* [Git Convention](docs/git.md)
* [API](docs/api.md)
* [Architecture Decision Records](docs/adr)

## ขั้นตอนการรันใน visual studio

    ใน solution มีหลาย Project เลือกที่ต้องการรัน

## ขั้นตอนการ build solution บน Terminal

    $ solution> dotnet build -tl

## ขั้นตอนการรัน project ใน Terminal

    $ project> dotnet run

## ขั้นตอนการรัน project ที่เป็น aspnetcore และ auto reload ใน Terminal

    $ project> dotnet watch run

## หน้าเว็บ (src/vuewebui)

หน้าเว็บใช้ Node.js รุ่น LTS (ตาม `.nvmrc`) และติดตั้งแพ็กเกจด้วย pnpm 12.9.1

    $ src\vuewebui> npx --yes pnpm@12.9.1 install
    $ src\vuewebui> npx --yes pnpm@12.9.1 dev

ข้อควรระวัง: pnpm 12 ไม่อ่านค่า `shamefully-hoist=true` ใน `.npmrc` แล้ว แพ็กเกจที่โค้ดเรา import ตรงๆ จึงต้องประกาศไว้ใน `package.json` เองเสมอ ห้ามพึ่งแพ็กเกจที่ติดมากับแพ็กเกจอื่น เช่น `flatpickr` ต้องเป็น dependency ตรง ถึงจะมี `vue-flatpickr-component` อยู่แล้วก็ตาม ไม่อย่างนั้น build จะพังเพราะหาไฟล์ของแพ็กเกจไม่เจอ

## เครื่องมือสร้างโค้ด Code Scaffolding

โปรเจกต์มี template ของตัวเองอยู่ที่ `templates/biglion-templates` ให้ติดตั้งครั้งเดียวต่อเครื่อง โดยรันจากโฟลเดอร์ solution

    $ solution> dotnet new install ./templates/biglion-templates

template นี้สร้างเฉพาะ Command หรือ Query พร้อม Validator และ Handler ส่วน EF configuration และ action ใน Controller ต้องเขียนเอง ไฟล์จะถูกสร้างในโฟลเดอร์ปัจจุบัน จึงต้อง `cd` เข้าโฟลเดอร์ปลายทางก่อน เช่น

    $ src\Application\Features\Patients\Commands> dotnet new biglion-command -n CreatePatient --featureName Patients --boundedContext Patients --returnType int

สำหรับ Query ใช้ `biglion-query` และต้องใส่ `--returnType` เองทุกครั้ง เพราะไม่มีค่าเริ่มต้น

ข้อควรระวัง: ไฟล์ที่ได้จะอยู่ในโฟลเดอร์ชื่อเดียวกับ `-n` (เช่น `CreatePatient`) และ namespace ยังมีคำว่า `BoundedContext` ติดมา หลังสร้างเสร็จต้องย้ายไฟล์เข้า `Commands/Create`, `Commands/Update`, `Commands/Delete` หรือ `Queries/Get` แล้วแก้ namespace เป็น `BigLion.CPA.Application.Features.{Feature}.Commands.Create` (หรือ `.Update`, `.Delete`, `.Queries.Get`) ให้ตรงโฟลเดอร์

รายละเอียดเพิ่มเติมดูที่ [.ai/ai-agent-guide.md](.ai/ai-agent-guide.md)

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

    1. ทำงานใน local folder D:\PROJECT\CPA เท่านั้น (ทุกเครื่องใช้ path เดียวกัน) branch หลักคือ master
    2. ห้าม push ขึ้น GitHub และไม่ใช้บริการ cloud ใดๆ กับ source code นี้ ทุกอย่างทำใน local repo
    3. สร้าง local branch ใหม่ที่แตกออกจาก master โดยตั้งชื่อตาม docs/git.md เช่น feat/patient-birthdate หรือ fix/flatpickr-dependency
    4. working tree ใน D:\PROJECT\CPA ใช้ร่วมกันหลายคน ใครจะสลับ branch (checkout/switch) ต้องแจ้งทีมก่อนทุกครั้ง เพราะจะกระทบงานที่คนอื่นทำค้างไว้
    5. พัฒนาคุณสมบัติส่วนที่เกี่ยวข้อง
    6. ตอน stage ให้ระบุไฟล์ทีละ path เสมอ (git add <path>) ห้ามใช้ git add -A, git add . หรือ git commit -a เพราะจะดึงไฟล์ migration ที่คุณ Teerapol ยังไม่ commit ติดไปด้วย
    7. commit การเปลี่ยนแปลง โดยที่หากเป็นการเปลี่ยนแปลงที่เกี่ยวข้องกับหลายไฟล์สามารถรวมกลุ่มเป็น commit เดียวกันได้ แต่ควรแยกส่วนให้ย่อยที่สุดหากทำได้
    8. การตั้งข้อความ commit พยายามให้สื่อความหมายถึงสิ่งที่เปลี่ยนแปลงและชัดเจนและเข้าใจง่ายเป็นภาษาไทย หรือภาษาอังกฤษ

## ก่อนการรวม source code เข้ากับ Branch หลัก

    1. ต้องมีการเขียน Unit Tests สำหรับ Domain Entity ก่อนเสมอ อย่างน้อยต้องมี test cases ที่ครอบคลุม constructor สำหรับ field ที่ required
    2. สำหรับ Command หรือ Query ต้องมีการเขียน Integration Tests อย่างน้อยครอบคลุม test cases สำหรับการทำงานที่สำเร็จ, การทำงานที่ไม่สำเร็จที่เกิดจากข้อมูลไม่มี, การทำงานที่ไม่สำเร็จที่เกิดจากการส่งข้อมูลมาไม่ตรงกับ Validation Rules หรือคุณสมบัติที่ค่อนข้างวิกฤตและสำคัญ
    3. การไม่เขียน tests สำหรับทดสอบ code ที่ตัวเองเขียนขึ้น เป็นการทำงานที่ไม่มีคุณภาพมีผลกระทบกับผู้ใช้งานอย่างสูง และสร้างความวิตกกังวลให้กับทีมเป็นอย่างมากในตอน Production
    4. การเขียน tests แม้จะไม่ได้การรันตีว่า Production จะไม่มี BUG 100% แต่ก็สามารถป้องกัน error บางอย่างที่ไม่ควรเกิดขึ้นและสามารถตรวจพบในระหว่างพัฒนาได้เลย ดีกว่าไปเจอที่ Production 

## การรวม source code เข้า master

    1. commit งานใน branch ของตัวเองให้ครบ โดย stage ทีละไฟล์ตามข้อ 6 ของขั้นตอนเข้าร่วมพัฒนา
    2. build solution ให้ผ่าน (dotnet build CPA.sln) และรันเทสให้ผ่านครบทั้ง 4 โปรเจกต์ (Domain.UnitTests, Application.UnitTests, Application.FunctionalTests, Infrastructure.IntegrationTests) ถ้าแก้หน้าเว็บด้วย ต้อง lint และ build ของ src/vuewebui ผ่านด้วย
    3. ถ้า master มี commit ใหม่ระหว่างที่ทำงาน ให้ rebase หรือ merge master เข้า branch ตัวเองใน local ก่อน (การสลับ branch ต้องแจ้งทีมก่อนตามข้อ 4 ของขั้นตอนเข้าร่วมพัฒนา) หากมี conflict ให้ resolve โดยใช้ DiffMerge และปรึกษาทีม
    4. แจ้ง Tech Lead ให้ review โดยบอกชื่อ branch Tech Lead จะดู diff เทียบกับ master ใน local repo และ test ก่อน
    5. merge เข้า master ได้หลังจากคุณ Teerapol อนุมัติแล้วเท่านั้น
    6. ห้าม push branch ใดๆ รวมถึง master ขึ้น GitHub
    7. หลัง merge แล้ว แตก branch ใหม่จาก master ล่าสุดเพื่อพัฒนาต่อ
    8. ทำตามขั้นตอนแรกวนไป

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
