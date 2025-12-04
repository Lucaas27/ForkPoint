import { api } from "./client";
//

export async function register(payload: any) {
	const res = await api.post("/api/auth/register", payload);
	return res.data;
}

export async function login(payload: any) {
	const res = await api.post("/api/auth/login", payload);
	const accessToken = res.data?.accessToken as string | undefined;
	const refresh = res.data?.refreshToken as string | undefined;
	if (accessToken) {
		try {
			localStorage.setItem("fp_token", accessToken);
			api.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
			if (refresh) localStorage.setItem("fp_refresh", refresh);
		} catch {
			console.error("Failed to store auth token");
		}
	}
	return res.data;
}

export async function logout() {
	const res = await api.post("/api/auth/logout");
	try {
		localStorage.removeItem("fp_token");
		delete api.defaults.headers.common.Authorization;
	} catch {
		console.error("Failed to remove auth token");
	}
	return res.data;
}

export async function refreshToken(payload: {
	accessToken: string;
	refreshToken: string;
}) {
	const res = await api.post("/api/auth/refresh-token", payload);
	const newAccessToken = res.data?.token as string | undefined;
	const newRefresh = res.data?.refreshToken as string | undefined;
	if (newAccessToken) {
		try {
			localStorage.setItem("fp_token", newAccessToken);
			api.defaults.headers.common.Authorization = `Bearer ${newAccessToken}`;
			if (newRefresh) localStorage.setItem("fp_refresh", newRefresh);
		} catch {}
	}
	return res.data;
}
