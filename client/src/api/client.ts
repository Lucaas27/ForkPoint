import { env } from "../env";
import axios from "axios";

const API_BASE = env.VITE_API_BASE_URL;

// Auth related request/response interceptors are registered by the
// AuthProvider at runtime so token state stays in memory within the provider.
export const api = axios.create({
	baseURL: API_BASE,
	withCredentials: true,
});
