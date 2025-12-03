import { createFileRoute } from '@tanstack/react-router'
import { useCreateRestaurant } from '../../features/restaurants/mutations'
import { useNavigate } from '@tanstack/react-router'
import { toast } from 'sonner'
import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Input } from '../../components/ui/input'
import { Label } from '../../components/ui/label'
import { Button } from '../../components/ui/button'
import { Textarea } from '../../components/ui/textarea'
import { Plus } from 'lucide-react'

export const Route = createFileRoute('/restaurants/create')({
    component: CreateRestaurant,
})

function CreateRestaurant() {
    const navigate = useNavigate()
    const [name, setName] = useState('')
    const [description, setDescription] = useState('')
    const [category, setCategory] = useState('')
    const mutation = useCreateRestaurant()

    return (
        <div className="max-w-2xl mx-auto">
            <Card>
                <CardHeader>
                    <CardTitle className="text-2xl">Create Restaurant</CardTitle>
                    <CardDescription>Add a new restaurant to the system</CardDescription>
                </CardHeader>
                <CardContent>
                    <form
                        className="space-y-4"
                        onSubmit={(e) => {
                            e.preventDefault()
                            mutation.mutate(
                                { name, description, category },
                                {
                                    onSuccess: () => {
                                        toast.success('Restaurant created')
                                        navigate({ to: '/restaurants' })
                                    },
                                    onError: (err: any) => toast.error(err?.message || 'Failed to create restaurant'),
                                },
                            )
                        }}
                    >
                        <div className="space-y-2">
                            <Label htmlFor="name">Restaurant Name</Label>
                            <Input
                                id="name"
                                placeholder="Enter restaurant name"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="description">Description</Label>
                            <Textarea
                                id="description"
                                placeholder="Describe the restaurant"
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                rows={4}
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="category">Category</Label>
                            <Input
                                id="category"
                                placeholder="e.g., Italian, Chinese, Burgers"
                                value={category}
                                onChange={(e) => setCategory(e.target.value)}
                            />
                        </div>

                        <Button type="submit" className="w-full" disabled={mutation.isPending}>
                            <Plus className="h-4 w-4 mr-2" />
                            {mutation.isPending ? 'Creating...' : 'Create Restaurant'}
                        </Button>
                    </form>
                </CardContent>
            </Card>
        </div>
    )
}
