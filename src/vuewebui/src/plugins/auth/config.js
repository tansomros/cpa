/* eslint-disable camelcase */

import Oidc from 'oidc-client'

export default {
  authority: import.meta.env.VITE_OIDC_AUTHORITY,
  client_id: import.meta.env.VITE_OIDC_CLIENT_ID,
  redirect_uri: import.meta.env.VITE_OIDC_REDIRECT_URI,
  response_type: import.meta.env.VITE_OIDC_RESPONSE_TYPE,
  scope: import.meta.env.VITE_OIDC_SCOPE,
  post_logout_redirect_uri: import.meta.env.VITE_OIDC_POST_LOGOUT_REDIRECT_URI,
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  automaticSilentRenew: true,
  silent_redirect_uri: import.meta.env.VITE_OIDC_SILENT_REDIRECT_URI,
  logging: { console: true, level: Oidc.Log.ERROR },
}
