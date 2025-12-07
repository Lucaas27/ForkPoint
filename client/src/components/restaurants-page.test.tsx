/* @vitest-environment jsdom */
import { render, screen, fireEvent, cleanup } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';

// Mock the Route hooks used internally by the component
const navigateMock = vi.fn();
const searchMock = { searchBy: 'Name', searchTerm: 'pizza', categoryFilter: 'Italian' };

vi.mock('@/routes/restaurants', () => ({
    Route: {
        useNavigate: () => navigateMock,
        useSearch: () => searchMock,
    },
}));

import { RestaurantsPager } from './restaurants-pager';

describe('RestaurantsPager component', () => {
    beforeEach(() => {
        cleanup();
        navigateMock.mockClear();
    });

    it('renders page summary and correct button states', () => {
        render(<RestaurantsPager currentPage={1} totalPages={3} totalItems={25} pageSize={10} />);

        expect(screen.getByText(/Page 1 of 3/i)).toBeTruthy();
        expect(screen.getByText(/Showing 1-10 of 25/i)).toBeTruthy();

        const prev = screen.getByRole('button', { name: /Previous/i }) as HTMLButtonElement;
        const next = screen.getByRole('button', { name: /Next/i }) as HTMLButtonElement;
        expect(prev.disabled).toBe(true);
        expect(next.disabled).toBe(false);
    });

    it('calls navigate with expected search params when clicking Next', () => {
        render(<RestaurantsPager currentPage={2} totalPages={4} totalItems={40} pageSize={10} />);

        const next = screen.getByRole('button', { name: /Next/i });
        fireEvent.click(next);

        expect(navigateMock).toHaveBeenCalledTimes(1);
        expect(navigateMock).toHaveBeenCalledWith({
            to: '/restaurants',
            search: expect.objectContaining({
                page: 3,
                size: 10,
                searchBy: 'Name',
                searchTerm: 'pizza',
                categoryFilter: 'Italian',
            }),
        });
    });
});
