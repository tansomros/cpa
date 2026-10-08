# ADR-017

## Lookup ข้อมูลพฤติกรรมสุขภาพของผู้ป่วยด้วย SmartEnum

Status : Accepted

Date : 2026-10-08

ผู้ตัดสินใจ : Teerapol

ADR ที่เกี่ยวข้อง : ADR-013 (Validation), ADR-014 (YAGNI)

---

## บริบท

ข้อมูลผู้ป่วย (`Patient`) มีช่องพฤติกรรมสุขภาพ 4 ช่องที่ต้องเลือกจากรายการตัวเลือก

| ช่องใน `Patient` | ชื่อใน Command / ViewModel | ความหมาย | ชนิดตอนนี้ |
|---|---|---|---|
| `Smoke` | `Smoke` | การสูบบุหรี่ | `int?` |
| `CigaretteType` | `CigaretteType` | ชนิดของบุหรี่ที่สูบ | `int?` |
| `Drinking` | `Alcohol` | การดื่มเครื่องดื่มแอลกอฮอล์ | `int?` |
| `DrinkFrequency` | `AlcoholFQ` | ความถี่ในการดื่ม | `int?` |

ตอนนี้ทั้ง 4 ช่องเป็นตัวเลขที่ไม่มีความหมายกำกับ หน้าเว็บ (`src/vuewebui/src/pages/patients/list/index.vue`) ให้พิมพ์ตัวเลขเองในช่อง `type="number"` และ API ยังไม่มีกฎตรวจค่าของช่องเหล่านี้

ระบบเดิมที่ CPA คัดลอกมาเก็บตัวเลือกแบบนี้ไว้ในตาราง `ReferenceGroup` / `ReferenceValue` ซึ่งถูกลบออกไปแล้วตอนล้างโค้ดที่เหลือจากระบบ Checkup

ในโค้ดมีกลไก lookup แบบ SmartEnum อยู่แล้ว

* `SmartEnum<T>` (`src/Domain/Common/SmartEnum.cs`) แต่ละค่ามี `Value` (รหัส), `Code`, `Name` (ชื่อภาษาไทยที่แสดง) และ `Sort` (ลำดับ) โดย `All` คืนทุกค่าเรียงตาม `Sort`, `FromValue` / `TryFromValue` แปลงรหัสกลับเป็นค่า SmartEnum และ `GetDisplayName(lang)` อ่านชื่อจากไฟล์ `.resx` ถ้ามี ถ้าไม่มีจะใช้ `Name` (ตอนนี้ยังไม่มีไฟล์ `.resx` จึงได้ `Name` เสมอ)
* `LookupRegistry` (`src/Application/Features/Lookups/LookupRegistry.cs`) เป็นที่ลงทะเบียน category ของ lookup โดยแต่ละ category ชี้ไปที่ SmartEnum หนึ่งตัว แล้วแปลงเป็น `LookupOptionDto(Value, DisplayName)`
* `GetLookupOptionsQuery` ดึงตัวเลือกของ category จาก `LookupRegistry` (ถ้าไม่พบ category จะ throw `NotFoundException`) ส่วน `GetLookupCategoriesQuery` คืนรายชื่อ category ทั้งหมด ทั้งสองตัวเรียกได้โดยไม่ต้อง login (`CpaPolicies.AllowAnonymous`)
* `OptionsController` มี `GET /Options` (รายชื่อ category) และ `GET /Options/{category}?lang=` (ตัวเลือกของ category นั้น)
* ฝั่งหน้าเว็บมี `useLookupStore` (`src/vuewebui/src/stores/useLookupStore.js`) ที่เรียก `/Options/{category}` และ cache ผลไว้

และ commit `7a0947b` (update enums) เพิ่ม `SmokingValue` และ `DrinkingValue` ไว้ใน `src/Domain/Enums/` แล้ว

---

## Decision

ใช้ **SmartEnum** เป็นรายการตัวเลือกของทั้ง 4 ช่อง

| ช่อง | SmartEnum |
|---|---|
| การสูบบุหรี่ (`Smoke`) | `SmokingValue` (มีแล้ว) |
| ชนิดบุหรี่ (`CigaretteType`) | SmartEnum ตัวใหม่ |
| การดื่ม (`Drinking`) | `DrinkingValue` (มีแล้ว) |
| ความถี่ในการดื่ม (`DrinkFrequency`) | SmartEnum ตัวใหม่ |

ไม่กลับไปใช้ตาราง `ReferenceGroup` / `ReferenceValue` ในฐานข้อมูล

ทั้ง 4 ตัวลงทะเบียนใน `LookupRegistry` และหน้าเว็บดึงตัวเลือกผ่าน `GET /Options/{category}` เหมือนกันทั้งหมด

ชนิดบุหรี่มีตัวเลือก "อื่นๆ" ถ้าเลือกข้อนี้ให้พิมพ์รายละเอียดในช่อง `SmokingRemark`

รายการรหัสจริงของแต่ละช่องยังรอยืนยัน ดูหัวข้อ "รายละเอียดที่รอยืนยัน" ด้านล่าง

---

# เหตุผล

* **ตัวเลือกแทบไม่เปลี่ยน** สถานะการสูบ ชนิดบุหรี่ การดื่ม และความถี่ในการดื่ม เป็นรายการที่นิ่ง ไม่จำเป็นต้องมีหน้าจอให้แก้
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

* คอลัมน์ `Smoke`, `CigaretteType`, `Drinking` และ `DrinkFrequency` ในตาราง `Patients` เปลี่ยนจาก `int?` เป็นข้อความ รวมถึง property ที่เกี่ยวข้องใน `Patient`, Command, ViewModel และหน้าเว็บ
* migration ของการเปลี่ยนนี้ Teerapol สร้างเอง ทีม AI ไม่สร้าง migration
* หน้าเว็บเปลี่ยนจากช่องพิมพ์ตัวเลขเป็นตัวเลือกที่ดึงจาก `/Options/{category}`
* การเพิ่มหรือลบตัวเลือกต้องแก้โค้ดและ deploy ใหม่ ซึ่งยอมรับได้เพราะตัวเลือกแทบไม่เปลี่ยน

---

## สิ่งที่โค้ดใน master ยังไม่ตรงกับ ADR นี้ (ณ commit `7a0947b`)

**`DrinkingValue` มีรหัสซ้ำ (bug)**

| field | `Value` | `Code` | `Name` | `Sort` |
|---|---|---|---|---|
| `Non` | `Non` | `N` | ไม่ดื่ม | 0 |
| `Quit` | `Quit` | `Y` | เคยดื่มแต่เลิกแล้ว | 1 |
| `Occasionally` | `Quit` | `Q` | ดื่มครั้งคราว | 2 |
| `Regularly` | `Quit` | `Q` | ดื่มประจำ | 2 |

สามค่าใช้ `Value` = `Quit` ร่วมกัน และสองค่าใช้ `Sort` = 2 ร่วมกัน ผลคือ

* `FromValue` / `TryFromValue` ของ `DrinkingValue` จะ error ทุกครั้ง เพราะ dictionary ที่สร้างจาก `Value` มี key ซ้ำ (ส่วน `All` ยังคืนครบ 4 ค่า)
* `Equals` มองว่า 3 ค่านี้เป็นค่าเดียวกัน เพราะเทียบจาก `Value`
* ถ้าบันทึกลงฐานข้อมูล จะแยกไม่ออกว่าเป็น "เคยดื่มแต่เลิกแล้ว", "ดื่มครั้งคราว" หรือ "ดื่มประจำ"

ต้องแก้ก่อนนำไปใช้

**เรื่องอื่นที่ยังต้องทำ**

* `SmokingValue` มี 3 ค่า คือ `Non` (ไม่สูบ), `Regularly` ที่ใช้ `Value` = `Yes` (สูบประจำ) และ `Quit` (เลิกสูบแล้ว)
* ยังไม่มี SmartEnum ของชนิดบุหรี่และความถี่ในการดื่ม
* `LookupRegistry` ยังว่าง ยังไม่มี category ไหนลงทะเบียน (มีแค่ comment ตัวอย่าง `smoking-statuses`)
* `OptionsController` ยังไม่ถูก compile เพราะ `src/API/API.csproj` มี `<Compile Remove="Controllers\OptionsController.cs" />` และไฟล์ยังใช้ namespace เก่า `Kondongpu.*` ตอนนี้ API จึงยังไม่มี `/Options` ต้องแก้ namespace และเอาบรรทัด `Compile Remove` ออกก่อน
* `SmartEnum<T>` ค้นรหัสแบบไม่สนตัวพิมพ์เล็กใหญ่ (`StringComparer.OrdinalIgnoreCase`) เช่น `TryFromValue("regular")` จะหาเจอ
* ทั้ง 4 ช่องใน `Patient` ยังเป็น `int?` และ `PatientWriteRules` ยังไม่มีกฎตรวจช่องเหล่านี้
* constructor ของ `SmokingValue` และ `DrinkingValue` ตั้งชื่อ parameter ตัวที่สองว่า `abnormalFlag` (ชื่อที่ติดมาจากระบบ Checkup) แต่ค่าที่ส่งเข้าไปคือ `Code` ของ base class
* entity `MTM` มีช่องชื่อคล้ายกัน (`Smoke`, `CigaretteType`, `Alcohol`, `AlcoholFQ` เป็น `int?`) ADR นี้ครอบคลุมเฉพาะ `Patient` ยังไม่ได้ตัดสินใจเรื่อง `MTM`

---

## รายละเอียดที่รอยืนยัน

> Status : Proposed
>
> หัวข้อนี้ยังไม่ใช่ข้อตกลง เป็นข้อเสนอที่รอ Teerapol ยืนยัน

### 1. รายการรหัส (ร่างจาก BA)

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
| `RollYourOwn` | บุหรี่มวนเอง/ยาเส้น |
| `Electronic` | บุหรี่ไฟฟ้า |
| `Other` | อื่นๆ (รายละเอียดใน `SmokingRemark`) |

การดื่ม (`Drinking`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `Non` | ไม่ดื่ม |
| `Quit` | เคยดื่มแต่เลิกแล้ว |
| `Occasional` | ดื่มครั้งคราว |
| `Regular` | ดื่มประจำ |

ความถี่ในการดื่ม (`DrinkFrequency`)

| รหัส | ชื่อที่แสดง |
|---|---|
| `LessThanMonthly` | น้อยกว่าเดือนละครั้ง |
| `Monthly` | 1–3 ครั้งต่อเดือน |
| `Weekly` | 1–4 ครั้งต่อสัปดาห์ |
| `Daily` | เกือบทุกวันหรือทุกวัน |

หมายเหตุ: comment ของ `DrinkFrequency` ใน `Patient` ตอนนี้เขียนว่า "ความถี่ในการดื่ม ครั้ง/สัปดาห์" ซึ่งเป็นแบบตัวเลข ต้องปรับตามเมื่อเปลี่ยนเป็นรหัส

### 2. เปลี่ยนรหัส `Yes` เป็น `Regular`

เปลี่ยนรหัสสูบประจำของ `SmokingValue` จาก `Yes` เป็น `Regular` ตอนนี้ ก่อนที่จะมีข้อมูลจริง

### 3. เก็บเฉพาะ `Value`

เก็บเฉพาะ `Value` ในคอลัมน์ `varchar(20)` และเอา `Code` ออกถ้าไม่มีที่ใช้ (ตอนนี้ `LookupRegistry` และ `LookupOptionDto` ใช้แค่ `Value` กับชื่อที่แสดง ไม่ได้ใช้ `Code`)

### 4. เลิกใช้ `IsSmoke` และ `SmokingQuit`

สองช่องนี้ความหมายซ้ำกับ `Smoke` จึงเสนอให้เลิกใช้ และดูจาก `Smoke` อย่างเดียว

หมายเหตุ: comment ของ `SmokingQuit` ใน `Patient` เขียนว่า "อยากจะลดหรือเลิกสูบบุหรี่หรือไม่" (ความตั้งใจจะเลิก) แต่หน้าเว็บติด label ว่า "เลิกบุหรี่แล้ว" ต้องยืนยันความหมายก่อนว่าซ้ำกับ `Smoke` = `Quit` จริงหรือไม่ ถ้าหมายถึงความตั้งใจจะเลิก ข้อมูลนี้จะหายไปถ้าเลิกใช้ช่องนี้

### 5. กฎข้ามช่อง

ใช้ทั้งใน validator ของ API และหน้าเว็บ (ตาม ADR-013: กฎหลักอยู่ที่ Backend ส่วนหน้าเว็บมีไว้เพื่อ UX)

* เลือกชนิดบุหรี่ได้เฉพาะเมื่อ `Smoke` เป็น `Regular` หรือ `Quit`
* เลือกความถี่ในการดื่มได้เฉพาะเมื่อ `Drinking` เป็น `Occasional` หรือ `Regular`
* เลือก `Non` แล้วให้ล้างช่องที่เกี่ยวข้อง
* `SmokeYear` และ `SmokeCigarette` ยังเป็นตัวเลขเหมือนเดิม

### 6. API ปฏิเสธรหัสที่ไม่มีใน SmartEnum

validator ของ API ไม่รับรหัสที่ไม่ได้กำหนดไว้ใน SmartEnum เช่น `Yes` แบบเดิม ตัวพิมพ์ไม่ตรง อย่าง `regular` หรือพิมพ์ผิด

หมายเหตุ: `TryFromValue` ไม่สนตัวพิมพ์เล็กใหญ่ ถ้าจะปฏิเสธ `regular` ต้องเทียบรหัสแบบตรงตัวเอง หรือเปลี่ยนการเทียบใน `SmartEnum<T>`

### 7. ชื่อที่ยังไม่ได้กำหนด

ชื่อ SmartEnum ตัวใหม่ (ชนิดบุหรี่ ความถี่ในการดื่ม) และชื่อ category ที่จะลงทะเบียนใน `LookupRegistry` ยังไม่ได้กำหนด
