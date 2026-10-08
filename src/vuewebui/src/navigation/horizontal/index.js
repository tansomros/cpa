// The horizontal menu mirrors the vertical one, so it reuses the same menu files.
import dashboard from '../vertical/dashboard'
import masterData from '../vertical/master-data'
import pharmacy from '../vertical/pharmacy'

export default [...dashboard, ...pharmacy, ...masterData]
