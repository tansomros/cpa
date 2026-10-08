// Thai Buddhist-era (B.E. / พ.ศ.) helpers for flatpickr date pickers.
// The UI shows and accepts B.E. dates as dd/mm/yyyy, while the value bound to
// v-model and sent to the API stays C.E. "yyyy-MM-dd" (no time, no timezone).
// All dates are built in local time, never through toISOString(), so the day never shifts.
// eslint-disable-next-line import/extensions
import { Thai } from 'flatpickr/dist/esm/l10n/th.js'

export const BE_OFFSET = 543

const ISO_FORMAT = 'Y-m-d'
const THAI_FORMAT = 'd/m/Y'

export const THAI_DATE_MESSAGES = Object.freeze({
  invalidFormat: 'รูปแบบวันที่ต้องเป็น วว/ดด/ปปปป',
  yearOutOfRange: 'กรุณากรอกปีเป็น พ.ศ.',
  afterMaxDate: 'วันเกิดต้องไม่เกินวันนี้',
})

export const THAI_DATE_PLACEHOLDER = 'วว/ดด/ปปปป'

const pad = n => String(n).padStart(2, '0')

export const startOfToday = () => {
  const today = new Date()

  today.setHours(0, 0, 0, 0)

  return today
}

// Same day `years` years before `from`; 29 Feb falls back to 28 Feb (matches DateOnly.AddYears).
export const yearsAgo = (years, from = startOfToday()) => {
  const date = new Date(from.getFullYear() - years, from.getMonth(), from.getDate())
  if (date.getMonth() !== from.getMonth())
    date.setDate(0)

  return date
}

const buildDate = (year, month, day) => {
  const date = new Date(2000, 0, 1)

  date.setFullYear(year, month - 1, day)

  const isRealDay = date.getFullYear() === year && date.getMonth() === month - 1 && date.getDate() === day

  return isRealDay ? date : undefined
}

export const toIsoDate = date => `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`

export const toThaiDate = date => `${pad(date.getDate())}/${pad(date.getMonth() + 1)}/${date.getFullYear() + BE_OFFSET}`

// "1987-03-15" or "1987-03-15T00:00:00" -> Date (local midnight)
export const parseIsoDate = value => {
  const match = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(value ?? '').trim())

  return match ? buildDate(Number(match[1]), Number(match[2]), Number(match[3])) : undefined
}

// "15/03/2530" (B.E. year; "/", "-" or "." separators, 1-2 digit day and month) -> Date
export const parseThaiDate = value => {
  const match = /^(\d{1,2})[/.-](\d{1,2})[/.-](\d{4})$/.exec(String(value ?? '').trim())

  return match ? buildDate(Number(match[3]) - BE_OFFSET, Number(match[2]), Number(match[1])) : undefined
}

// For tables and read-only views: C.E. value from the API -> "15/03/2530", empty -> ''
export const formatThaiDate = value => {
  const date = value instanceof Date ? value : parseIsoDate(value)

  return date ? toThaiDate(date) : ''
}

const nativeInputProperty = name => Object.getOwnPropertyDescriptor(HTMLInputElement.prototype, name)

// flatpickr reads and writes the header year input (value/min/max) in C.E.
// Present those numbers in B.E. without touching flatpickr's internal year, so the
// year arrows, typing a B.E. year and month navigation across years all stay correct.
const patchYearInput = input => {
  if (!input || input.dataset.thaiYear === 'true')
    return

  const initial = {}

  for (const name of ['value', 'min', 'max']) {
    const native = nativeInputProperty(name)

    initial[name] = native.get.call(input)
    Object.defineProperty(input, name, {
      configurable: true,
      get() {
        const raw = native.get.call(this)
        const year = Number.parseInt(raw, 10)

        // Partial typing such as "25" is passed through untouched.
        return year >= 1000 + BE_OFFSET ? String(year - BE_OFFSET) : raw
      },
      set(value) {
        const year = Number.parseInt(value, 10)

        native.set.call(this, Number.isNaN(year) ? value : String(year + BE_OFFSET))
      },
    })
  }

  for (const name of ['value', 'min', 'max']) {
    if (initial[name] !== '')
      input[name] = initial[name]
  }

  input.dataset.thaiYear = 'true'
}

const patchYearInputs = instance => {
  const inputs = instance?.yearElements?.length ? instance.yearElements : [instance?.currentYearElement]

  inputs.forEach(patchYearInput)
}

/**
 * flatpickr config for a Thai B.E. date field.
 * v-model stays C.E. "yyyy-MM-dd" (null/'' when empty); the input and calendar header show B.E.
 *
 * @param {object} options
 * @param {Date} [options.minDate] earliest allowed date
 * @param {Date} [options.maxDate] latest allowed date (default: today)
 * @param {(message: string) => void} [options.onError] called with a message for bad input, '' when valid again
 * @param {object} [options.messages] overrides for THAI_DATE_MESSAGES
 */
export const createThaiDatePickerConfig = ({ minDate, maxDate = startOfToday(), onError, messages = {} } = {}) => {
  const text = { ...THAI_DATE_MESSAGES, ...messages }
  let parseFailed = false

  const report = message => onError?.(message)

  const fail = message => {
    parseFailed = true
    report(message)

    return undefined
  }

  const parseDate = (value, format) => {
    // Values coming from v-model / the API are C.E. ISO dates.
    if (format === ISO_FORMAT)
      return parseIsoDate(value)

    const date = parseThaiDate(value)
    if (!date)
      return fail(text.invalidFormat)

    const year = date.getFullYear()
    if ((minDate && date < minDate) || (maxDate && year > maxDate.getFullYear()))
      return fail(text.yearOutOfRange)
    if (maxDate && date > maxDate)
      return fail(text.afterMaxDate)

    report('')

    return date
  }

  return {
    locale: { ...Thai, firstDayOfWeek: 0 },
    dateFormat: ISO_FORMAT,
    altInput: true,
    altFormat: THAI_FORMAT,
    allowInput: true,
    disableMobile: true,
    minDate,
    maxDate,
    formatDate: (date, format) => (format === ISO_FORMAT ? toIsoDate(date) : toThaiDate(date)),
    parseDate,
    onReady: [(_dates, _str, instance) => patchYearInputs(instance)],
    onOpen: [(_dates, _str, instance) => patchYearInputs(instance)],
    onChange: [
      selectedDates => {
        // A picked date, or a cleared field, is valid; keep the message after a failed parse.
        if (selectedDates.length || !parseFailed)
          report('')
        parseFailed = false
      },
    ],
  }
}