export default [
  {
    title: 'ข้อมูลหลัก',
    icon: { icon: 'tabler-database' },
    children: [
      { title: 'ข้อมูลฝ่าย', to: 'divisions-list' },
      { title: 'ข้อมูลแผนก', to: 'departments-list' },
      { title: 'อาคาร/ตึก', to: 'buildings-list' },
      { title: 'ข้อมูลชั้น', to: 'floors-list' },
      { title: 'ข้อมูลสถานที่', to: 'locations-list' },
      { title: 'ข้อมูลบริษัท/ผู้ค้า', to: 'companies-list' },
      { title: 'ข้อมูลหน่วยนับ', to: 'unit-of-measures-list' },
      { title: 'ข้อมูลสินค้า/รายการ', to: 'contract-products-list' },
    ],
  },
]
