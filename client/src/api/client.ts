import { env } from '../env'
import axios from 'axios';

const API_BASE = env.VITE_API_BASE_URL

export const api = axios.create({
  baseURL: API_BASE,
  withCredentials: true,
})

export const setAuthToken = (token?: string) => {
  if (token) {
    api.defaults.headers.common.Authorization = `Bearer ${token}`
  } else {
    delete api.defaults.headers.common.Authorization
  }
}
