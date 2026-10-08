## Git Convention 
คือข้อตกลงร่วมกันในการใช้งาน Git เพื่อให้ประวัติการแก้ไขโค้ด (Git History) สะอาด อ่านง่าย ค้นหาจุดผิดพลาดย่อยๆ ได้รวดเร็ว และลดปัญหาโค้ดชนกัน (Merge Conflicts) ในทีมครับ [1] 
หัวข้อหลักๆ ที่ทีมพัฒนาต้องตกลงร่วมกัน มีดังนี้:
------------------------------
## 1. รูปแบบการเขียนข้อความ Commit (Commit Message Convention)
นิยมใช้มาตรฐาน Conventional Commits ซึ่งกำหนดให้เขียนในรูปแบบ:
type(scope): description [2, 3, 4, 5] 

* type (ประเภทการแก้ไข):
* feat: เพิ่มฟีเจอร์ใหม่ (Feature) เช่น feat(auth): add google login
   * fix: แก้ไขบั๊ก (Bug Fix) เช่น fix(cart): repair total price calculation
   * docs: แก้ไขเฉพาะเอกสาร เช่น docs: update readme file
   * style: แก้ไขรูปแบบโค้ด ลบช่องว่าง (ไม่มีผลต่อการทำงานของโค้ด) เช่น style: format program.cs
   * refactor: ปรับปรุงโครงสร้างโค้ดภายใน ไม่ได้เพิ่มฟีเจอร์หรือแก้บั๊ก เช่น refactor(db): optimize user query
   * test: เพิ่มหรือแก้ไข Unit Test เช่น test: add user service test
   * chore: งานทั่วไปที่ไม่เกี่ยวกับโค้ดหลัก เช่น อัปเดตแพ็กเกจ เปลี่ยนเวอร์ชันโปรเจกต์ [6, 7, 8] 
* description (คำอธิบาย):
* ใช้คำกริยานำ (Imperative mood) เช่น ใช้ add แทน added, ใช้ fix แทน fixed
   * เขียนสั้นๆ กระชับ และเป็นภาษาอังกฤษ (ถ้าในทีมตกลงกันไว้) [9] 

------------------------------
## 2. การตั้งชื่อกิ่ง (Branch Naming Convention)
กิ่งที่สร้างขึ้นมาเพื่อทำงานควรขึ้นต้นด้วยประเภทของงาน ตามด้วยหมายเลข Ticket (ถ้ามี) และคำอธิบายสั้นๆ โดยแยกคำด้วยเครื่องหมายขีดกลาง (-)

* ฟีเจอร์ใหม่: feature/JIRA-123-user-profile หรือ feat/user-profile
* แก้ไขบั๊กทั่วไป: bugfix/cart-item-dup หรือ fix/cart-item-dup
* แก้ไขบั๊กด่วนบนโปรดักชัน: hotfix/payment-crash

------------------------------
## 3. โครงสร้างและการบริหารกิ่ง (Git Workflow)
ทีมนี้ทำงานแบบ local เท่านั้น ใน D:\PROJECT\CPA (ทุกเครื่องใช้ path เดียวกัน) ไม่ push ขึ้น GitHub และไม่ใช้ Pull Request บน server ขั้นตอนเต็มดูที่ README.md หัวข้อ "ขั้นตอนเข้าร่วมพัฒนา" และ "การรวม source code เข้า master":

   1. master: เป็นกิ่งหลักที่เก็บโค้ดที่ทำงานได้จริงและพร้อมขึ้นโปรดักชันเสมอ ห้าม Commit โค้ดตรงๆ เข้ากิ่งนี้เด็ดขาด
   2. feature-branches: เมื่อจะทำงาน ให้แตก local branch ออกจาก master ไปทำงานของตัวเอง (ใครจะสลับ branch ใน working tree ที่ใช้ร่วมกัน ต้องแจ้งทีมก่อน)
   3. Review และ Merge: เมื่อทำเสร็จ build และเทสต้องผ่านครบ ให้ Tech Lead รีวิวโค้ด (Code Review) แล้วจึง merge กลับเข้า master หลังคุณ Teerapol อนุมัติ

------------------------------
## 4. การจัดการไฟล์ที่ไม่จำเป็นด้วย .gitignore
ห้าม Push ไฟล์ที่สร้างขึ้นมาระหว่างการคอมไพล์ โฟลเดอร์ของระบบ หรือไฟล์ข้อมูลส่วนตัว (เช่น รหัสผ่าน, Connection String) ขึ้น Git เด็ดขาด
สำหรับโปรเจกต์ C# / .NET ให้สร้างไฟล์ชื่อ .gitignore ไว้ที่ Root โฟลเดอร์ เพื่อสั่งให้ Git ข้ามไฟล์เหล่านี้อัตโนมัติ: [15] 

# ยอดฮิตสำหรับ C# / .NET
[Ob]in/
[Ob]bj/
*.user
*.suo
*.userosscache
*.sln.docstates

# ไฟล์ตั้งค่าส่วนตัว (รหัสผ่านฐานข้อมูล ห้ามเอาขึ้น Git)
appsettings.Development.json
secrets.json
.env

# โฟลเดอร์เครื่องมือและ IDE
.vs/
.vscode/

------------------------------ 