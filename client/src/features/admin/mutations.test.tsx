import { describe, it, expect, vi, beforeEach } from "vitest";
import { render, fireEvent, waitFor } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

vi.mock("@/api/admin", () => ({
    assignRole: vi.fn(() => Promise.resolve({ data: {} })),
    removeRole: vi.fn(() => Promise.resolve({ data: {} })),
}));

import { useAssignRole, useRemoveRole } from "./mutations";

function createQueryClient() {
    return new QueryClient({ defaultOptions: { queries: { retry: false } } });
}

describe("admin mutations", () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it("useAssignRole invalidates users queries on success", async () => {
        const qc = createQueryClient();
        const spy = vi.spyOn(qc, "invalidateQueries");

        function TestComp() {
            const m = useAssignRole(false);
            return (
                <button type="button" onClick={() => m.mutate({ email: "a@b.com", role: "Owner" })}>go</button>
            );
        }

        const { getByText } = render(
            <QueryClientProvider client={qc}>
                <TestComp />
            </QueryClientProvider>,
        );

        fireEvent.click(getByText("go"));

        await waitFor(() => {
            expect(spy).toHaveBeenCalled();
        });
    });

    it("useRemoveRole invalidates users queries on success", async () => {
        const qc = createQueryClient();
        const spy = vi.spyOn(qc, "invalidateQueries");

        function TestComp() {
            const m = useRemoveRole(false);
            return (
                <button type="button" onClick={() => m.mutate({ email: "a@b.com", role: "Owner" })}>go</button>
            );
        }

        const { getByText } = render(
            <QueryClientProvider client={qc}>
                <TestComp />
            </QueryClientProvider>,
        );

        fireEvent.click(getByText("go"));

        await waitFor(() => {
            expect(spy).toHaveBeenCalled();
        });
    });
});
