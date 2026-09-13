// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

export const toBuddhistYear = (m, format) => {
  var christianYear = m.format("YYYY")
  var buddhishYear = (parseInt(christianYear)+543).toString()

  return m
    .format(format.replace("YYYY", buddhishYear).replace("YY", buddhishYear.substring(2, 4)))
    .replace(christianYear, buddhishYear)
}

export const getAgeTextFromBirthDate = date => {
  var b = moment(date), a = moment(new Date())
  var diffYear = a.diff(b, "years")
  b.add(diffYear, "years")
  var diffMonth = a.diff(b, "months")
  b.add(diffMonth, "months")
  var diffDay = a.diff(b, "days")
  b.add(diffDay, "days")

  var date = {
    years: diffYear,
    months: diffMonth,
    days: diffDay,
  }

  var text = ""
  if (date.years > 0) {
    text += date.years + " ปี "
  }

  if (date.months > 0) {
    text += date.months + " เดือน "
  }

  if (date.days > 0) {
    text += date.days + " วัน"
  }

  date.text = text

  var shortText = ""
  if (date.years == 0 && date.months == 0 && date.days >= 0) {
    shortText += date.days + " วัน"
  }

  if (date.years == 0 && date.months > 0) {
    shortText += date.months + " เดือน"
  }

  if (date.years > 0) {
    shortText += date.years + " ปี"
  }

  date.shortText = shortText
  
  return date
}
