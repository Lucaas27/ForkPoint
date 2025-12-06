import { env } from "../env";
import axios from "axios";
import { refreshToken as doRefreshToken } from "./auth";

const API_BASE = env.VITE_API_BASE_URL;

export const api = axios.create({
	baseURL: API_BASE,
	withCredentials: true,
});

// Initialize auth header from token in localStorage (if any)
try {
	const t = localStorage.getItem("fp_token");
	if (t) api.defaults.headers.common.Authorization = `Bearer ${t}`;
} catch {
	console.error("Failed to set auth token");
}

/**
 * Axios response interceptor to handle authentication errors and token refresh.
 *
 * This interceptor catches 401 Unauthorized responses and attempts to refresh the access token
 * using the stored refresh token. If successful, it retries the original request with the new token.
 * If refresh fails or no tokens are available, it clears stored tokens and rejects the request.
 *
 * @param response - The successful response object.
 * @param error - The error object from the failed request.
 * @returns The original response if successful, or a rejected promise with the error.
 */
type AxiosErrorShape = {
	config?: { _retry?: boolean; headers?: Record<string, string> };
	response?: { status?: number };
};

api.interceptors.response.use(
	(response) => response,
	async (error: unknown) => {
		const err = error as AxiosErrorShape;
		const originalRequest = err?.config;
		const status = err?.response?.status;

		if (status === 401 && originalRequest && !originalRequest._retry) {
			originalRequest._retry = true;
			try {
				const accessToken = localStorage.getItem("fp_token");
				const refreshToken = localStorage.getItem("fp_refresh");
				if (!refreshToken || !accessToken)
					throw new Error("No refresh token available");
				const refreshRes = await doRefreshToken({ accessToken, refreshToken });
				const newAccess = refreshRes?.token as string | undefined;
				if (!newAccess) throw new Error("Failed to refresh token");
				// doRefreshToken already updated localStorage and axios defaults.
				// retry original request with new token
				originalRequest.headers = {
					...(originalRequest.headers || {}),
					Authorization: `Bearer ${newAccess}`,
				};
				return api.request(
					originalRequest as unknown as import("axios").AxiosRequestConfig,
				);
			} catch {
				// Clear tokens on failure
				localStorage.removeItem("fp_token");
				localStorage.removeItem("fp_refresh");
				delete api.defaults.headers.common.Authorization;

				return Promise.reject(err);
			}
		}
		return Promise.reject(err);
	},
);
