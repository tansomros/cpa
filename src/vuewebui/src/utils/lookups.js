/**
 * Smart Enum value constants -- mirrors the Value property from the .NET Domain layer.
 * Used for type-safe comparisons in conditionals. Display names come from the API.
 *
 * These values rarely change. If a new Smart Enum value is added on the backend,
 * add the corresponding entry here.
 */

export const ExamResult = Object.freeze({
  Normal: 'Normal',
  Abnormal: 'Abnormal',
  NotExamined: 'ไม่ได้ตรวจ',
})

export const LabResult = Object.freeze({
  Normal: 'Normal',
  Abnormal: 'Abnormal',
})

export const XrayResult = Object.freeze({
  Normal: 'Normal',
  Abnormal: 'Abnormal',
  WaitForSpecialist: 'W',
})

export const BmdResult = Object.freeze({
  Normal: 'Normal',
  OsteopeniaRisk: 'C1',
  Osteoporosis: 'C2',
})

export const AbiResult = Object.freeze({
  Normal: 'Normal',
  Arteriosclerosis: 'C1',
  Occlusion: 'C2',
})

export const CheckupStatus = Object.freeze({
  Pending: '1',
  InProgress: '2',
  Reported: '3',
})

export const EyeResult = Object.freeze({
  Normal: 'ปกติ',
  Farsighted: 'สายตายาว',
  Nearsighted: 'สายตาสั้น',
})

export const HearingLossLevel = Object.freeze({
  Normal: 'ปกติ',
  Mild: 'เสียเล็กน้อย',
  Moderate: 'เสียปานกลาง',
  Severe: 'เสียมาก',
  Profound: 'เสียรุนแรง',
})

/**
 * All available lookup categories matching the API endpoint names.
 */
export const LookupCategories = Object.freeze({
  ExamResults: 'exam-results',
  LabResults: 'lab-results',
  XrayResults: 'xray-results',
  BmdResults: 'bmd-results',
  AbiResults: 'abi-results',
  CheckupStatuses: 'checkup-statuses',
  EyeResults: 'eye-results',
  HearingLossLevels: 'hearing-loss-levels',
})
