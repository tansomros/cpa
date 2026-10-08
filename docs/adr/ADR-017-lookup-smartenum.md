# ADR-017

## Lookup ข้อมูลพฤติกรรมสุขภาพของผู้ป่วยด้วย SmartEnum

Status : Accepted

Date : 2026-10-08

แก้ไข : 2026-10-09 ใช้ SmartEnum กับ 3 ช่อง (ความถี่ในการดื่มเป็นตัวเลขจำนวนวันต่อสัปดาห์), เอา `IsSmoke` ออก, `MTM` ไม่เก็บข้อมูลการสูบและการดื่มแล้ว และ Teerapol ตอบครบทุกข้อในหัวข้อ "รายละเอียดการตัดสินใจ"

ผู้ตัดสินใจ : Teerapol

ADR ที่เกี่ยวข้อง : ADR-013 (Validation), ADR-014 (YAGNI)

---

## บริบท

ข้อมูลผู้ป่วย (`Patient`) มีช่องพฤติกรรมสุขภาพ 4 ช่อง

| ช่องใน `Patient` | ชื่อใน Command / ViewModel | ความหมาย | ชนิดตอนนี้ |
|---|---|---|---|
| `Smoke` | `Smoke` | การสูบบุหรี่ | `int?` |
| `CigaretteType` | `CigaretteType` | ชนิดของบุหรี่ที่สูบ | `int?` |
| `Drinking` | `Alcohol` | การดื่มเครื่องดื่มแอลกอฮอล์ | `int?` |
| `DrinkFrequency` | `AlcoholFQ` | ความถี่ในการดื่ม | `int?` |

ตอนนี้ทั้ง 4 ช่องเป็นตัวเลขที่ไม่มีความหมายกำกับ หน้าเว็บ (`src/vuewebui/src/pages/patients/list/index.vue`) ให้พิมพ์ตัวเลขเองในช่อง `type="number"` และ API ยังไม่มีกฎตรวจค่าของช่องเหล่านี้

นอกจากนี้ `Patient` มี `IsSmoke` และ `SmokingQuit` (`bool?`) และ entity `MTM` มีช่องการสูบและการดื่มของตัวเองอีก 6 ช่อง คือ `Smoke`, `SmokeYear`, `SmokeCigarette`, `CigaretteType`, `Alcohol` และ `AlcoholFQ` (`int?` ทั้งหมด)

ระบบเดิมที่ CPA คัดลอกมาเก็บตัวเลือกแบบนี้ไว้ในตาราง `ReferenceGroup` / `ReferenceValue` ซึ่งถูกลบออกไปแล้วตอนล้างโค้ดที่เหลือจากระบบ Checkup

ในโค้ดมีกลไก lookup แบบ SmartEnum อยู่แล้ว

* `SmartEnum<T>` (`src/Domain/Common/SmartEnum.cs`) แต่ละค่ามี `Value` (รหัส), `Name` (ชื่อภาษาไทยที่แสดง) และ `Sort` (ลำดับ) โดย `All` คืนทุกค่าเรียงตาม `Sort`, `FromValue` / `TryFromValue` แปลงรหัสกลับเป็นค่า SmartEnum และ `GetDisplayName(lang)` อ่านชื่อจากไฟล์ `.resx` ถ้ามี ถ้าไม่มีจะใช้ `Name` (ตอนนี้ยังไม่มีไฟล์ `.resx` จึงได้ `Name` เสมอ)
* `LookupRegistry` (`src/Application/Features/Lookups/LookupRegistry.cs`) เป็นที่ลงทะเบียน category ของ lookup โดยแต่ละ category ชี้ไปที่ SmartEnum หนึ่งตัว แล้วแปลงเป็น `LookupOptionDto(Value, DisplayName)`
* `GetLookupOptionsQuery` ดึงตัวเลือกของ category จาก `LookupRegistry` (ถ้าไม่พบ category จะ throw `NotFoundException`) ส่วน `GetLookupCategoriesQuery` คืนรายชื่อ category ทั้งหมด ทั้งสองตัวเรียกได้โดยไม่ต้อง login (`CpaPolicies.AllowAnonymous`)
* `OptionsController` มี `GET /Options` (รายชื่อ category) และ `GET /Options/{category}?lang=` (ตัวเลือกของ category นั้น)
* ฝั่งหน้าเว็บมี `useLookupStore` (`src/vuewebui/src/stores/useLookupStore.js`) ที่เรียก `/Options/{category}` และ cache ผลไว้

และ commit `7a0947b` (update enums) เพิ่ม `SmokingValue` และ `DrinkingValue` ไว้ใน `src/Domain/Enums/` แล้ว

---

## Decision

ใช้ **SmartEnum** เป็นรายการตัวเลือกของ 3 ช่อง

| ช่อง | SmartEnum |
|---|---|
| การสูบบุหรี่ (`Smoke`) | `SmokingValue` |
| ชนิดบุหรี่ (`CigaretteType`) | `CigaretteTypeValue` |
| การดื่ม (`Drinking`) | `DrinkingValue` |

ความถี่ในการดื่ม (`DrinkFrequency`) ไม่ใช่ตัวเลือก เก็บจำนวนวันที่ดื่มต่อสัปดาห์เป็นจำนวนเต็ม `int?` (0–7)

ข้อมูลการสูบและการดื่มเก็บที่ `Patient` ที่เดียว `MTM` ไม่เก็บแล้ว (ดูข้อ 6)

> เดิมตัดสินใช้ SmartEnum กับ 4 ช่อง (รวมความถี่ในการดื่ม) และให้ `MTM` เก็บรหัสชุดเดียวกัน Teerapol เปลี่ยนเป็นแบบด้านบนเมื่อ 2026-10-09

ไม่กลับไปใช้ตาราง `ReferenceGroup` / `ReferenceValue` ในฐานข้อมูล

ทั้ง 3 ตัวลงทะเบียนใน `LookupRegistry` และหน้าเว็บดึงตัวเลือกผ่าน `GET /Options/{category}` เหมือนกันทั้งหมด โดยใช้ `useLookupStore.getOptions(category)` เป็นทางเดียวในการโหลดตัวเลือก

ชนิดบุหรี่มีตัวเลือก "อื่นๆ" ถ้าเลือกข้อนี้ให้พิมพ์รายละเอียดในช่อง `SmokingRemark`

รายละเอียดแต่ละข้ออยู่ในหัวข้อ "รายละเอียดการตัดสินใจ" ด้านล่าง

---

## เหตุผล

* **ตัวเลือกแทบไม่เปลี่ยน** สถานะการสูบ ชนิดบุหรี่ และการดื่ม เป็นรายการที่นิ่ง ไม่จำเป็นต้องมีหน้าจอให้แก้
* **ตัวเลือกผูกกับกฎและรายงาน** ค่าเหล่านี้ใช้เป็นเงื่อนไขใน validation และใช้นับในรายงาน ถ้าเก็บในตารางที่แก้หรือลบได้ การแก้ข้อมูลในฐานข้อมูลอาจทำให้รายงานย้อนหลังผิด และทำให้เงื่อนไขในโค้ดพังเงียบ ๆ โดย compiler ไม่เตือน ส่วน SmartEnum ถ้าเปลี่ยนชื่อหรือลบค่า compiler จะแจ้งทุกจุดที่ใช้
* **ใช้กลไกที่มีอยู่แล้ว** ใช้ `LookupRegistry` และ `/Options/{category}` ที่มีอยู่ ไม่ต้องออกแบบตาราง หน้าจอจัดการ หรือ API ใหม่
* **ชื่อภาษาไทยอยู่ที่เดียว** ชื่อที่แสดงอยู่ในโค้ด ไม่ขึ้นกับข้อมูลในฐานข้อมูล ข้อนี้สำคัญตอนนี้ที่งานทำกันหลายเครื่อง และข้อมูลในฐานข้อมูลของแต่ละเครื่องไม่เหมือนกัน
* **ทดสอบง่าย** เขียน test ตัวเดียวไล่ทุก category ที่ลงทะเบียนไว้ได้ ว่ารหัส (`Value`) และ `Sort` ไม่ซ้ำ และทุกค่ามีชื่อภาษาไทย

---

## การเก็บค่าในฐานข้อมูล

คอลัมน์เก็บ **รหัสที่เป็นข้อความและไม่เปลี่ยน** คือ `Value` ของ SmartEnum (เช่น `Non`, `Quit`)

ไม่เก็บ

* ข้อความภาษาไทย (เช่น "ไม่สูบ") เพราะชื่อที่แสดงปรับคำได้
* ตัวเลข เพราะเปิดดูข้อมูลในฐานข้อมูลแล้วไม่รู้ความหมาย

ดังนั้นชื่อภาษาไทย (`Name`) และลำดับการแสดง (`Sort`) แก้ได้ตลอดโดยไม่กระทบข้อมูลที่บันทึกไว้แล้ว สิ่งที่ห้ามเปลี่ยนหลังมีข้อมูลจริงคือ `Value`

---

## ถ้าวันหนึ่งต้องให้ Admin แก้ตัวเลือกได้

ถ้ามี category ไหนที่ต้องให้ Admin เพิ่มหรือแก้ตัวเลือกเองจริง ๆ ให้เปลี่ยนเฉพาะ category นั้นให้ `LookupRegistry` อ่านจากตาราง ส่วน category อื่นยังเป็น SmartEnum เหมือนเดิม

หน้าเว็บยังเรียก `GET /Options/{category}` เหมือนเดิม ไม่ต้องแก้

ตอนนี้ยังไม่ทำ (YAGNI ตาม ADR-014)

---

## ผลที่ตามมา

* `Smoke`, `CigaretteType` และ `Drinking` ใน `Patient` เปลี่ยนจาก `int?` เป็นข้อความ `varchar(20)` รวมถึง property ที่เกี่ยวข้องใน Command, ViewModel และหน้าเว็บ ส่วน `DrinkFrequency` ยังเป็น `int?` (จำนวนวันที่ดื่มต่อสัปดาห์)
* `Patient` ไม่มี `IsSmoke` แล้ว
* `MTM` เอาออก 6 คอลัมน์ คือ `Smoke`, `SmokeYear`, `SmokeCigarette`, `CigaretteType`, `Alcohol` และ `AlcoholFQ`
* migration ของการเปลี่ยนนี้ Teerapol สร้างเอง ทีม AI ไม่สร้าง migration
* หน้าเว็บเปลี่ยน 3 ช่องจากช่องพิมพ์ตัวเลขเป็นตัวเลือกที่ดึงจาก `/Options/{category}`
* การเพิ่มหรือลบตัวเลือกต้องแก้โค้ดและ deploy ใหม่ ซึ่งยอมรับได้เพราะตัวเลือกแทบไม่เปลี่ยน
* ค่าเดิมในฐานข้อมูลไม่แปลงเป็นค่าใหม่ ซึ่งไม่เป็นปัญหา เพราะ Teerapol จะสร้างข้อมูลใหม่อยู่แล้ว

---

## สิ่งที่โค้ดใน master ยังไม่ตรงกับ ADR นี้ (ณ commit `17b4d2e`)

**สิ่งที่ `17b4d2e` แก้แล้ว**

* เอา `Code` ออกจาก `SmartEnum<T>` (constructor เหลือ `value`, `name`, `sort`) และเอา parameter `abnormalFlag` ออกจาก `SmokingValue` และ `DrinkingValue`
* `DrinkingValue` ไม่มีรหัสซ้ำแล้ว คือ `Non`, `Quit`, `Occasional` และ `Regular`
* เปลี่ยนชื่อ field เป็น `Regular` และ `Occasional` (เดิม `Regularly` และ `Occasionally`)

**งานบน branch `feat/patient-lifestyle-lookup` (ยังไม่ merge)**

master ยังไม่มีงานด้านล่าง เช่น `SmokingValue.Regular` ยังเก็บ `Yes` และ `SmartEnumTests.cs` ยังส่ง code ทำให้ Domain.UnitTests ใน master build ไม่ผ่าน

* `1b70b0e` รหัส `Yes` เป็น `Regular`, `Sort` ของ `DrinkingValue.Regular` เป็น 3 และ `SmartEnumTests.cs` ไม่ส่ง code แล้ว
* `9063c76` เปลี่ยน `Smoke`, `CigaretteType` และ `Drinking` / `Alcohol` ใน `Patient` และ `MTM` เป็น `string?` ยาวไม่เกิน 20 (ตอนนั้นรวม `DrinkFrequency` / `AlcoholFQ` ด้วย ซึ่ง `fd7a825` เปลี่ยนกลับ)
* `5956470` เพิ่ม `CigaretteTypeValue`, ลงทะเบียน lookup ใน `LookupRegistry` และเปิด `OptionsController` กลับมา (namespace `BigLion.CPA.Presentation.API.Controllers`, เอา `Compile Remove` ออก)
* `fd7a825` ให้ `DrinkFrequency` / `AlcoholFQ` กลับเป็น `int?` และเอา `DrinkFrequencyValue` ออก ตอนนี้ `LookupRegistry` มี 3 category
* `9154261` หน้า Patient มี dropdown 3 ช่องที่โหลดจาก `/options/{category}`
* `949a0e2` Teerapol แก้ entity เอง: เอา `IsSmoke` ออกจาก `Patient` และเอา 6 ช่องการสูบและการดื่มออกจาก `MTM` (Developer commit ให้ตามที่แก้)
* `1825f78` แก้ให้ build ผ่าน: เอา `IsSmoke` ออกจาก `UpdateSmokingHistory`, Command, ViewModel, `PatientConfiguration`, หน้า Patient และ test และเอา 6 ช่องของ `MTM` ออกจาก Command, ViewModel และ `MTMConfiguration`
* `a35a2e4` ข้อ 4: ล้างค่าใน Domain, validator ตรวจความถี่ 0–7 และ `Regular` ต้องไม่น้อยกว่า 1, หน้า Patient ใช้ `useLookupStore.getOptions` แสดงข้อความ error เมื่อโหลดตัวเลือกไม่สำเร็จ ซ่อนและล้างช่องที่ไม่เกี่ยวข้อง แก้ label ของ `SmokingQuit` และใช้หน่วย "วัน/สัปดาห์"
* `49da516` ข้อ 5: ปฏิเสธรหัส `Smoke`, `CigaretteType` และ `Alcohol` ที่ไม่รู้จักหรือตัวพิมพ์ไม่ตรง (เทียบแบบ ordinal) แยกเป็น commit ต่างหาก

ขั้นต่อไป

* QA เพิ่ม test เป็น commit แยก (FunctionalTests จะยังไม่ผ่านจนกว่า Teerapol สร้าง migration)
* เมื่อ QA ผ่านและ Tech Lead อนุมัติ Developer merge เข้า master ในเครื่องด้วย `--no-ff` ไม่ push
* หลัง merge Teerapol สร้าง migration ใหม่ ระหว่างนี้ master จะยังไม่ตรงกับฐานข้อมูล `cpathai`

---

## รายละเอียดการตัดสินใจ

> Status : Accepted
>
> Teerapol ตอบครบทุกข้อเมื่อ 2026-10-09 ("ทำตามที่แนะนำ") ทุกข้อด้านล่างจึงเป็นข้อตกลงแล้ว

### 1. รายการรหัสและชื่อภาษาไทย

รหัสใช้ถาวร เปลี่ยนไม่ได้หลังมีข้อมูลจริง ส่วนชื่อภาษาไทยแก้คำได้ภายหลัง ชื่อที่แสดงด้านล่างตรงกับโค้ดบน branch

การสูบบุหรี่ (`Smoke`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Non` | ไม่สูบ |
| `Regular` | สูบประจำ |
| `Quit` | เลิกสูบแล้ว |

ชนิดบุหรี่ (`CigaretteType`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Manufactured` | บุหรี่ซอง |
| `RollYourOwn` | ยาเส้นมวนเอง |
| `Electronic` | บุหรี่ไฟฟ้า |
| `Other` | อื่นๆ |

ถ้าเลือก `Other` ให้พิมพ์รายละเอียดใน `SmokingRemark`

การดื่ม (`Drinking`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Non` | ไม่ดื่ม |
| `Quit` | เคยดื่มแต่เลิกแล้ว |
| `Occasional` | ดื่มครั้งคราว |
| `Regular` | ดื่มประจำ |

### 2. เลิกใช้ `IsSmoke` และเก็บ `SmokingQuit` ไว้

* `IsSmoke` ความหมายซ้ำกับ `Smoke` เอาออกจาก `Patient` แล้ว ดูจาก `Smoke` อย่างเดียว
* `SmokingQuit` หมายถึง "อยากลดหรือเลิกสูบบุหรี่" (ยืนยันในข้อ 7) เก็บไว้ ถามเฉพาะเมื่อ `Smoke` = `Regular` และเปลี่ยน label บนหน้าเว็บเป็น "อยากลดหรือเลิกสูบบุหรี่" (`a35a2e4`)

### 3. รหัสสูบประจำและการเก็บค่า (ทำแล้ว)

* ใช้รหัส `Regular` แทน `Yes` ใน `SmokingValue` (`1b70b0e`)
* เก็บเฉพาะ `Value` ในคอลัมน์ `varchar(20)` (รหัสที่ยาวที่สุดคือ `Manufactured` 12 ตัวอักษร)
* เอา `Code` ออก (master `17b4d2e`)

### 4. กฎข้ามช่อง (ทำแล้วใน `a35a2e4`)

* `CigaretteType`, `SmokeYear` และ `SmokeCigarette` กรอกได้เฉพาะเมื่อ `Smoke` เป็น `Regular` หรือ `Quit` ถ้าเป็น `Non` ให้ล้างเป็น null
* `SmokingQuit` ใช้เฉพาะเมื่อ `Smoke` = `Regular` นอกนั้นล้างเป็น null (คนที่เปลี่ยนจาก `Regular` เป็น `Quit` จะไม่ค้างค่า "อยากเลิก")
* `DrinkFrequency` คือจำนวนวันที่ดื่มต่อสัปดาห์ เป็นจำนวนเต็ม 0–7 ไม่มีทศนิยมหรือค่าติดลบ ถ้า `Drinking` เป็น `Regular` ต้องกรอกและไม่น้อยกว่า 1, ถ้าเป็น `Occasional` ไม่บังคับและกรอก 0 ได้, ถ้าเป็น `Non` หรือ `Quit` ให้ล้างเป็น null หน่วยบนหน้าเว็บเป็น "วัน/สัปดาห์"

กฎอยู่ที่ไหน (ตาม Tech Lead)

* การล้างค่าเป็น null ทำใน Domain คือ `Patient.UpdateSmokingHistory` และ `Patient.UpdateAlcoholHistory`
* validator ของ API ปฏิเสธเฉพาะ `Drinking` = `Regular` ที่ไม่กรอกความถี่หรือกรอก 0, ความถี่นอกช่วง 0–7 (`InclusiveBetween`) และรหัสที่ไม่รู้จัก (ข้อ 5) ค่าที่ส่งมาเกินแต่แค่ต้องล้าง ไม่ถือเป็น error เพราะ Domain ล้างให้เอง
* หน้าเว็บซ่อนช่องที่ไม่เกี่ยวข้องและล้างค่าเมื่อเปลี่ยนสถานะ เพื่อช่วยผู้ใช้เท่านั้น (ตาม ADR-013)

### 5. API ปฏิเสธรหัสที่ไม่มีใน SmartEnum (ทำแล้วใน `49da516`)

validator ของ API ไม่รับรหัสที่ไม่ได้กำหนดไว้ใน SmartEnum เช่น `Yes` แบบเดิม ตัวพิมพ์ไม่ตรงอย่าง `regular` หรือพิมพ์ผิด

`TryFromValue` ของ `SmartEnum<T>` ไม่สนตัวพิมพ์เล็กใหญ่ จึงให้ตรวจตัวพิมพ์แบบตรงตัวใน validator ของ `Patient` **ไม่แก้ `SmartEnum<T>`** เพราะเป็น base class ที่ใช้ร่วมกัน ถ้าแก้จะกระทบส่วนอื่น

### 6. `MTM` ไม่เก็บข้อมูลการสูบและการดื่ม (เปลี่ยนจากข้อเสนอเดิม)

Teerapol ตัดสินเมื่อ 2026-10-09 ให้เก็บข้อมูลการสูบและการดื่มที่ `Patient` ที่เดียว เพื่อไม่ให้ซ้ำซ้อน จึงเอา `Smoke`, `SmokeYear`, `SmokeCigarette`, `CigaretteType`, `Alcohol` และ `AlcoholFQ` ออกจาก `MTM` (`949a0e2`, `1825f78`)

ข้อแลกเปลี่ยนที่ BA และ Tech Lead ยกขึ้นมา และ Teerapol ยอมรับ

* `MTM` แสดงประวัติการสูบและการดื่มของแต่ละครั้งที่มารับบริการไม่ได้แล้ว (เช่น ความคืบหน้าในการเลิกบุหรี่)
* ค่าในช่องเหล่านี้ของ `MTM` จากระบบเดิมจะไม่ถูกย้ายมา

### 7. ความหมายของ `SmokingQuit` (ยืนยันแล้ว)

Teerapol ยืนยันว่า `SmokingQuit` หมายถึง "อยากลดหรือเลิกสูบบุหรี่" ตรงกับ comment ใน `Patient` และ label บนหน้าเว็บเปลี่ยนจาก "เลิกบุหรี่แล้ว" เป็น "อยากลดหรือเลิกสูบบุหรี่" แล้ว (`a35a2e4`)

### 8. ชื่อ category ใน `LookupRegistry` (รายละเอียดทางเทคนิคสำหรับผู้พัฒนา ตัดสินโดย Tech Lead)

| category | SmartEnum |
|---|---|
| `smoking` | `SmokingValue` |
| `cigarette-type` | `CigaretteTypeValue` |
| `drinking` | `DrinkingValue` |

`CigaretteTypeValue` สร้างใน `5956470` ส่วน `DrinkFrequencyValue` และ category `drink-frequency` เอาออกใน `fd7a825`

### 9. comment ของ `DrinkFrequency` (รายละเอียดทางเทคนิคสำหรับผู้พัฒนา ทำแล้วใน `a35a2e4`)

comment ของ `DrinkFrequency` ใน `Patient` แก้จาก "ความถี่ในการดื่ม ครั้ง/สัปดาห์" เป็น "ความถี่ในการดื่ม วัน/สัปดาห์" แล้ว
