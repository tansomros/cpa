import re

file_path = r'd:\Project\Kondongpu\src\vuewebui\src\pages\companies\list\index.vue'

with open(file_path, 'r', encoding='utf-8') as f:
    content = f.read()

# Replace API endpoints
content = content.replace('/Venders/GetAllVendorList', '/Companies')
content = content.replace('/Venders/', '/Companies/')
content = content.replace('/Venders', '/Companies')

# Replace array property
content = content.replace('listData.value?.vendors', 'listData.value?.companies')

# Replace payload
payload_old = '''    const payload = {
      name: editItemName.value,
      nameEnglish: editItemNameEnglish.value,
      sapCode: editItemSapCode.value,
      tax: editItemTax.value || null,
      address: editItemAddress.value,
      provinceName: editItemProvinceName.value,
      districtName: editItemDistrictName.value,
      subDistrictName: editItemSubDistrictName.value,
      postalCode: editItemPostalCode.value,
      phone: editItemPhone.value,
      fax: editItemFax.value || null,
      email: editItemEmail.value || null
    }'''

payload_new = '''    const payload = {
      name: editItemName.value,
      aliasName: editItemNameEnglish.value,
      companyCode: editItemSapCode.value,
      ownerName: editItemName.value,
      vatId: editItemTax.value || null,
      addressNo: editItemAddress.value,
      provinceId: editItemProvinceName.value,
      city: editItemDistrictName.value,
      district: editItemSubDistrictName.value,
      zipCode: editItemPostalCode.value,
      telephone: editItemPhone.value,
      fax: editItemFax.value || null,
      email: editItemEmail.value || null
    }'''
content = content.replace(payload_old, payload_new)

# Map openEditDialog mappings
edit_mapping_old = '''  editItemName.value = item.name
  editItemNameEnglish.value = item.nameEnglish
  editItemSapCode.value = item.sapCode
  editItemTax.value = item.tax
  editItemAddress.value = item.address
  editItemProvinceName.value = item.provinceName
  editItemDistrictName.value = item.districtName
  editItemSubDistrictName.value = item.subDistrictName
  editItemPostalCode.value = item.postalCode
  editItemPhone.value = item.phone
  editItemFax.value = item.fax
  editItemEmail.value = item.email'''

edit_mapping_new = '''  editItemName.value = item.name
  editItemNameEnglish.value = item.aliasName
  editItemSapCode.value = item.companyCode
  editItemTax.value = item.vatId
  editItemAddress.value = item.addressNo
  editItemProvinceName.value = item.provinceName
  editItemDistrictName.value = item.city
  editItemSubDistrictName.value = item.district
  editItemPostalCode.value = item.zipCode
  editItemPhone.value = item.telephone
  editItemFax.value = item.fax
  editItemEmail.value = item.email'''
content = content.replace(edit_mapping_old, edit_mapping_new)

# Table items properties
content = content.replace('item.sapCode', 'item.companyCode')
content = content.replace('item.nameEnglish', 'item.aliasName')
content = content.replace('item.tax', 'item.vatId')
content = content.replace('item.address', 'item.addressNo')
content = content.replace('item.districtName', 'item.city')
content = content.replace('item.subDistrictName', 'item.district')
content = content.replace('item.postalCode', 'item.zipCode')
content = content.replace('item.phone', 'item.telephone')
content = content.replace('item.lastModified', 'item.mWhen')

# The computed property filter
filter_old = 'i => i.name?.toLowerCase().includes(q) || i.nameEnglish?.toLowerCase().includes(q) || i.sapCode?.toLowerCase().includes(q)'
filter_new = 'i => i.name?.toLowerCase().includes(q) || i.aliasName?.toLowerCase().includes(q) || i.companyCode?.toLowerCase().includes(q)'
content = content.replace(filter_old, filter_new)

# Headers key map
content = content.replace('key: "sapCode"', 'key: "companyCode"')
content = content.replace('key: "nameEnglish"', 'key: "aliasName"')
content = content.replace('key: "phone"', 'key: "telephone"')
content = content.replace('key: "lastModified"', 'key: "mWhen"')

# Template named slots
content = content.replace('#item.sapCode=', '#item.companyCode=')
content = content.replace('#item.nameEnglish=', '#item.aliasName=')
content = content.replace('#item.phone=', '#item.telephone=')
content = content.replace('#item.lastModified=', '#item.mWhen=')

# Template view mapping
view_old = '''<span class="text-body-1">{{ viewItem.sapCode }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ชื่อบริษัท (ไทย)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.name }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ชื่อบริษัท (อังกฤษ)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.nameEnglish }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  เลขประจำตัวผู้เสียภาษี
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.tax ?? '-' }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ที่อยู่
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.address }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  จังหวัด
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.provinceName }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  อำเภอ/เขต
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.districtName }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ตำบล/แขวง
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.subDistrictName }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  รหัสไปรษณีย์
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.postalCode }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  โทรศัพท์
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.phone }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  แฟกซ์
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.fax ?? '-' }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  อีเมล
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.email ?? '-' }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ปรับปรุงล่าสุดเมื่อ
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">
                    {{ viewItem.lastModified ? toBuddhistYear(moment(viewItem.lastModified), "LLL") : '-' }}
                  </span>'''

view_new = '''<span class="text-body-1">{{ viewItem.companyCode }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ชื่อบริษัท (ไทย)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.name }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ชื่อบริษัท (อังกฤษ)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.aliasName }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  เลขประจำตัวผู้เสียภาษี
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.vatId ?? '-' }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ที่อยู่
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.addressNo }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  จังหวัด
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.provinceName }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  อำเภอ/เขต
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.city }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ตำบล/แขวง
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.district }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  รหัสไปรษณีย์
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.zipCode }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  โทรศัพท์
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.telephone }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  แฟกซ์
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.fax ?? '-' }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  อีเมล
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.email ?? '-' }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ปรับปรุงล่าสุดเมื่อ
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">
                    {{ viewItem.mWhen ? toBuddhistYear(moment(viewItem.mWhen), "LLL") : '-' }}
                  </span>'''
content = content.replace(view_old, view_new)

# Replace remaining vendor wording
content = content.replace('vendors_highlights', 'companies_highlights')
content = content.replace('ผู้ค้า', 'บริษัท/ผู้ค้า')

with open(file_path, 'w', encoding='utf-8') as f:
    f.write(content)

print("Replacement complete.")
