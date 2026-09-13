import { UserManager } from 'oidc-client'
import config from './config'

const userManager = new UserManager(config)

export const userService = {
  signin,
  signinSilent,
  signout,
  completeSignin,
  completeSignout,
  getUser,
  getAccessToken,
}

async function signin() {
  try {
    await userManager.signinRedirect()
  } catch(error) {
    console.error("Sign-in redirect failed", error)
    throw error
  }
}

async function signinSilent() {
  try {
    await userManager.signinSilent()
  } catch(error) {
    console.error("Silent sign-in redirect failed", error)
    throw error
  }
}

async function signout() {
  try {
    await userManager.signoutRedirect()
  } catch(error) {
    console.error("Sign-out redirect failed", error)
    throw error
  }
}

async function completeSignin() {
  try {
    return await userManager.signinRedirectCallback()
  } catch(error) {
    console.error("Complete Sign-in failed", error)
    throw error
  }
}

async function completeSignout() {
  try {
    await userManager.signoutRedirectCallback()
  } catch(error) {
    console.error("Complete Sign-out failed", error)
    throw error
  }
}

async function getUser() {
  return await userManager.getUser()
}

async function getAccessToken() {
  const user = await userManager.getUser()
  
  return user?.access_token || null
}
  