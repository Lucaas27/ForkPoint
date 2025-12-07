import { describe, it, expect, vi, beforeEach } from "vitest";

// Mock the axios instance
vi.mock("./client", () => ({
	api: {
		post: vi.fn(() => Promise.resolve({ data: {} })),
	},
}));

import { api } from "./client";

beforeEach(() => {
	vi.clearAllMocks();
});

describe("auth API", () => {
	it("register posts to the auth register endpoint", async () => {
		const { register } = await import("./auth");
		const payload = { email: "a@b.com", password: "secret" };
		await register(payload);
		expect(api.post).toHaveBeenCalledWith("/api/auth/register", payload);
	});
});
