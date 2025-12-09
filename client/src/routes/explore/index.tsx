import { Card, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';
import { useExternalRestaurants } from '@/features/external/queries';
import { createFileRoute } from '@tanstack/react-router'
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { ChevronRight } from 'lucide-react';
import { PaginationControl } from '@/components/pagination-control';

type PaginationParams = {
    page: number;
    size: number;
}

type ExternalRestaurant = {
    id: string;
    name: string;
    description: string;
    category?: string;
    imageUrl?: string;
    website?: string;
}


export const Route = createFileRoute('/explore/')({
    component: Explore,
    validateSearch: (search: PaginationParams) => {
        const page = Number(search.page) || 1;
        const size = Number(search.size) || 10;

        return { page, size };
    },
})

function Explore() {
    const { page, size }: PaginationParams = Route.useSearch();
    const { data, isLoading, error } = useExternalRestaurants(page, size);
    const restaurants = data?.items ?? [];
    const currentPage = Number(page) || 1;
    const totalPages = data?.totalPages;
    const navigate = Route.useNavigate();


    if (isLoading) {
        return (
            <div className="space-y-4">
                <Skeleton className="h-12 w-full" />
                <Skeleton className="h-32 w-full" />
                <Skeleton className="h-32 w-full" />
            </div>
        );
    }

    if (error) {
        return (
            <Card className="border-destructive">
                <CardHeader>
                    <CardTitle className="text-destructive">Error</CardTitle>
                    <CardDescription>Failed to load restaurants</CardDescription>
                </CardHeader>
            </Card>
        );
    }


    return (
        <div className="space-y-6">
            <div className="flex items-center justify-between">
                <div>
                    <h2 className="text-3xl font-bold tracking-tight">Dining and Drinks</h2>
                    <p className="text-muted-foreground">Browse all available restaurants</p>
                </div>
            </div>
            <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
                {restaurants.map((r: ExternalRestaurant) => {
                    const fallback = r.imageUrl ?? `https://picsum.photos/seed/${encodeURIComponent(String(r.id))}/1200/800`;
                    return (
                        <Card key={r.id} className="hover:shadow-lg transition-all overflow-hidden">
                            <div className="aspect-video w-full bg-muted">
                                <img src={fallback} alt={r.name} className="h-full w-full object-cover" loading="lazy" />
                            </div>
                            <CardHeader>
                                <div className="flex items-start justify-between">
                                    <div className="space-y-1">
                                        <CardTitle>{r.name}</CardTitle>
                                        <CardDescription className="line-clamp-2">
                                            {r.description}
                                        </CardDescription>
                                        {r.category && <Badge variant="secondary">{r.category}</Badge>}
                                    </div>
                                    <a href={r.website ?? '#'} target="_blank" rel="noopener noreferrer">
                                        <Button variant="ghost" size="sm">
                                            View
                                            <ChevronRight className="h-4 w-4 ml-1" />
                                        </Button>
                                    </a>
                                </div>
                            </CardHeader>
                        </Card>
                    );
                })}
            </div>
            <PaginationControl
                currentPage={currentPage}
                totalPages={totalPages}
                onPageChange={(page) =>
                    navigate({
                        to: '/explore',
                        replace: true,
                        search: { page, size } as any
                    })
                }
            />
        </div >)
}

