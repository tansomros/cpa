export default [
    {
    title: 'ข้อมูลร้านยา',
    icon: { icon: 'tabler-smart-home' },
    to: 'pharmacies-list',
    },    
    {
        title: 'Patient',
        icon: { icon: 'tabler-users' },
        to: 'patients-list',
    },
    {
        title: 'รายการกิจกรรม',
        icon: { icon: 'tabler-list' },
       
    },
    {
        title: 'รายงาน',
        icon: { icon: 'tabler-align-box-left-stretch' },
        children: [
            { title: 'รายงานสรุปจำนวนกิจกรรม', },
            { title: 'รายงานรายชื่อผู้เข้ารับบริการแยกตามกิจกรรม',  },
        ],

    },
]
