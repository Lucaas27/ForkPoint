import { Button } from "@/components/ui/button";
import { Route } from "@/routes/restaurants";

export function RestaurantsPager({
    currentPage,
    totalPages,
    totalItems,
    pageSize,
}: {
    currentPage: number;
    totalPages: number;
    totalItems: number;
    pageSize: number;
}) {
    const navigate = Route.useNavigate();
    const { searchBy, searchTerm, categoryFilter } = Route.useSearch();

    const canPrev = currentPage > 1;
    const canNext = currentPage < totalPages;
    const start = (currentPage - 1) * pageSize + 1;
    const end = Math.min(currentPage * pageSize, totalItems);

    const searchParams = {
        page: currentPage,
        size: pageSize,
        ...(searchBy && { searchBy }),
        ...(searchTerm && { searchTerm }),
        ...(categoryFilter && { categoryFilter })
    };

    return (
        <div className="flex items-center justify-between pt-2">
            <div className="text-sm text-muted-foreground">
                Page {currentPage} of {totalPages} - Showing {start}-{end} of{" "}
                {totalItems}
            </div>
            <div className="flex items-center gap-2">
                <Button
                    variant="outline"
                    size="sm"
                    disabled={!canPrev}
                    onClick={() =>
                        navigate({
                            to: "/restaurants",
                            search: { ...searchParams, page: 1 },
                        })
                    }
                >
                    First
                </Button>
                <Button
                    variant="outline"
                    size="sm"
                    disabled={!canPrev}
                    onClick={() =>
                        navigate({
                            to: "/restaurants",
                            search: { ...searchParams, page: currentPage - 1 },
                        })
                    }
                >
                    Previous
                </Button>
                <Button
                    variant="outline"
                    size="sm"
                    disabled={!canNext}
                    onClick={() =>
                        navigate({
                            to: "/restaurants",
                            search: { ...searchParams, page: currentPage + 1 },
                        })
                    }
                >
                    Next
                </Button>
                <Button
                    variant="outline"
                    size="sm"
                    disabled={!canNext}
                    onClick={() =>
                        navigate({
                            to: "/restaurants",
                            search: { ...searchParams, page: totalPages },
                        })
                    }
                >
                    Last
                </Button>
            </div>
        </div>
    );
}