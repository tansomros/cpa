import settings from './settings'
import dashboard from './dashboard'
import masterData from './master-data'
import pharmacy from './pharmacy'

export default [...dashboard, ...pharmacy, ...masterData, ...settings]
