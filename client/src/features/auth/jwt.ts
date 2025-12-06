import { jwtDecode } from "jwt-decode";

export type Decoded = {
	exp?: number;
	[key: string]: any;
};

export function decode(token: string): Decoded | undefined {
	try {
		return jwtDecode<Decoded>(token);
	} catch {
		return undefined;
	}
}

/**
 * Checks if a JWT token is expired.
 *
 * This function decodes the provided JWT token and compares its expiration time
 * (from the `exp` claim) against the current time. If no token is provided,
 * it is considered expired. If the token has no `exp` claim, it is considered expired.
 *
 * @param token - The JWT token string to check. Optional; if omitted, returns true.
 * @returns True if the token is expired or invalid, false otherwise.
 */
export function isExpired(token?: string) {
	if (!token) return true;
	const payload = decode(token);
	if (!payload?.exp) return true;
	const now = Math.floor(Date.now() / 1000);
	return now >= payload.exp;
}

/**
 * Extracts roles from a JWT token.
 *
 * This function decodes the provided JWT token and attempts to retrieve roles
 * from the payload using the claim key. It checks for Microsoft identity claims role key. If a value is found, it returns it as an array of strings (converting if necessary). If no roles are found or the token is invalid, an empty array is returned.
 *
 * @param token - The JWT token string to extract roles from. Optional; if omitted, returns an empty array.
 * @returns An array of role strings, or an empty array if none found or invalid.
 */

// Backend creates roles using ClaimTypes.Role
const ROLE_CLAIM =
	"http://schemas.microsoft.com/ws/2008/06/identity/claims/role" as const;

export function rolesFromToken(token?: string): string[] {
	if (!token) return [];
	const payload = decode(token);
	if (!payload) return [];
	const role = payload["role"] ?? payload[ROLE_CLAIM];

	if (Array.isArray(role)) return role.map(String);
	if (typeof role === "string") return [role];
	return [];
}

export function fullNameFromToken(token?: string): string | undefined {
	if (!token) return undefined;
	const payload = decode(token);
	if (!payload) return undefined;
	const claim: Array<unknown> = [payload["unique_name"]];

	for (const c of claim) {
		if (typeof c === "string" && c.trim().length > 0) return c;
	}
	return undefined;
}
