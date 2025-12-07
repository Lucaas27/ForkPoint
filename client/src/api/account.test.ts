import { describe, it, expect, vi, beforeEach } from "vitest";

// Mock the axios instance
vi.mock("./client", () => ({
	api: {
		get: vi.fn(() => Promise.resolve({ data: {} })),
		post: vi.fn(() => Promise.resolve({ data: {} })),
		patch: vi.fn(() => Promise.resolve({ data: {} })),
	},
}));

import { api } from "./client";

beforeEach(() => {
	vi.clearAllMocks();
});

describe("account API", () => {
	it("updateMe patches account update endpoint", async () => {
		const { updateMe } = await import("./account");
		const payload = { displayName: "Bob" };
		await updateMe(payload);
		expect(api.patch).toHaveBeenCalledWith("/api/account/update", payload);
	});

	it("adminUpdateUser patches account update with id", async () => {
		const { adminUpdateUser } = await import("./account");
		const payload = { roles: ["Admin"] };
		await adminUpdateUser(11, payload);
		expect(api.patch).toHaveBeenCalledWith("/api/account/update/11", payload);
	});

	it("forgotPassword posts to forgot-password", async () => {
		const { forgotPassword } = await import("./account");
		const payload = { email: "a@b.com" };
		await forgotPassword(payload);
		expect(api.post).toHaveBeenCalledWith(
			"/api/account/forgot-password",
			payload,
		);
	});

	it("resetPassword posts to reset-password", async () => {
		const { resetPassword } = await import("./account");
		const payload = {
			email: "a@b.com",
			token: "t",
			password: "p",
			confirmPassword: "p",
		};
		await resetPassword(payload);
		expect(api.post).toHaveBeenCalledWith(
			"/api/account/reset-password",
			payload,
		);
	});

	it("confirmEmail calls verify with params", async () => {
		const { confirmEmail } = await import("./account");
		const params = { token: "abc", email: "a@b.com" };
		await confirmEmail(params);
		expect(api.get).toHaveBeenCalledWith("/api/account/verify", { params });
	});

	it("resendEmailConfirmation posts to resend-email-confirmation", async () => {
		const { resendEmailConfirmation } = await import("./account");
		const payload = { email: "a@b.com" };
		await resendEmailConfirmation(payload);
		expect(api.post).toHaveBeenCalledWith(
			"/api/account/resend-email-confirmation",
			payload,
		);
	});

	it("myRestaurants gets account restaurants", async () => {
		const { myRestaurants } = await import("./account");
		await myRestaurants();
		expect(api.get).toHaveBeenCalledWith("/api/account/restaurants");
	});
});
