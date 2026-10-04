export default [
  {
    title: 'ข้อมูลหลัก',
    icon: { icon: 'tabler-database' },
    children: [     
        { title: 'ผู้ใช้งาน', to: 'users-list', icon: { icon: 'tabler-user-cog' } },
        { title: 'ข้อมูลคำนำหน้าชื่อ', to: 'prefixs-list', icon: { icon: 'tabler-user' } },
        { title: 'กลุ่มร้านยา', to: 'pharmacy-groups-list', icon: { icon: 'tabler-category' } },
        { title: 'ประเภทร้านยา', to: 'pharmacy-types-list', icon: { icon: 'tabler-tags' } },     
    ],
  },
]
