export const emptyPharmacyForm = () => ({
  code: '',
  name: '',
  licenseNo: null,
  nhsoCode: null,
  name2: null,
  pharmacyGroupId: null,
  pharmacyTypeId: null,
  pharmacyTypeOther: null,
  addressNo: null,
  provinceId: null,
  districtId: null,
  subDistrictId: null,
  zipCode: null,
  fda_Province: null,
  office_Tel: null,
  office_Fax: null,
  office_Mail: null,
  lineID: null,
  co_Name: null,
  co_Mail: null,
  co_Tel: null,
  regisYear: null,
  lat: null,
  lng: null,
  isActive: true,
})

export const pharmacyToForm = pharmacy => {
  const form = emptyPharmacyForm()

  Object.keys(form).forEach(key => {
    if (pharmacy[key] !== undefined)
      form[key] = pharmacy[key]
  })

  return form
}

const blankToNull = value => (typeof value === 'string' && value.trim() === '' ? null : value)

export const formToPayload = form => {
  const payload = {}

  Object.entries(form).forEach(([key, value]) => {
    payload[key] = blankToNull(value)
  })
  payload.code = form.code?.trim() ?? ''
  payload.name = form.name?.trim() ?? ''

  return payload
}

export const extractValidationErrors = error => error?.data?.errors ?? {}
