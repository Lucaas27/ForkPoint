import {
    Pagination,
    PaginationContent,
    PaginationItem,
    PaginationLink,
    PaginationPrevious,
    PaginationNext,
    PaginationEllipsis,
} from "@/components/ui/pagination";

type Props = {
    currentPage: number;
    totalPages: number;
    onPageChange: (page: number) => void;
};

// create an inclusive range of numbers
function makeRange(start: number, end: number) {
    const out: number[] = [];

    for (let i = start; i <= end; i++) {
        out.push(i)
    };

    return out;
}

export function PaginationControl({ currentPage, totalPages, onPageChange }: Props) {
    // nothing to render when there is only a single page
    if (totalPages <= 1) return null;

    // helper to handle clicks on page links
    // prevent navigation and invoke onPageChange
    const handlePageClick = (page: number) => (e?: React.MouseEvent) => {
        e?.preventDefault();
        if (page === currentPage) return;
        onPageChange(page);
    };

    // windowSize controls how many pages to show around the current page
    const windowSize = 2;
    const startPage = Math.max(1, currentPage - windowSize);
    const endPage = Math.min(totalPages, currentPage + windowSize);

    // build a list of page items including numbers and ellipses
    type PageItem = number | "...";

    const pageItems: PageItem[] = [];

    // always show the first page and an ellipsis if there is a gap
    if (startPage > 1) {
        // push first page
        pageItems.push(1);
        // push ellipsis if there is a gap
        if (startPage > 2) {
            pageItems.push("...");
        }
    }

    // add the pages around the current page
    pageItems.push(...makeRange(startPage, endPage));

    // always show the last page and an ellipsis if there is a gap
    if (endPage < totalPages) {
        // push ellipsis if there is a gap
        if (endPage < totalPages - 1) {
            pageItems.push("...");
        }

        // push last page
        pageItems.push(totalPages);
    }

    return (
        <Pagination aria-label="Pagination">
            <PaginationContent>
                <PaginationItem>
                    <PaginationPrevious className="cursor-pointer" onClick={handlePageClick(Math.max(1, currentPage - 1))} />
                </PaginationItem>

                {pageItems.map((pageItem, idx) => (
                    // using a page number or ellipsis+index
                    <PaginationItem key={typeof pageItem === "number" ? `page-${pageItem}` : `ellipsis-${idx}`}>
                        {pageItem === "..." ? (
                            <PaginationEllipsis />
                        ) : (
                            <PaginationLink href="#" isActive={pageItem === currentPage} onClick={handlePageClick(pageItem as number)}>
                                {pageItem}
                            </PaginationLink>
                        )}
                    </PaginationItem>
                ))}

                <PaginationItem >
                    <PaginationNext className="cursor-pointer" onClick={handlePageClick(Math.min(totalPages, currentPage + 1))} />
                </PaginationItem>
            </PaginationContent>
        </Pagination>
    );
}

export default PaginationControl;
