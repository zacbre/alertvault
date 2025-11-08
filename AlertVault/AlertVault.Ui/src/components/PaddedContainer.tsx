interface Props {
    children: React.ReactNode;
}

export const PaddedContainer = ({ children }: Props) => {
    return (
        <div className="p-4 bg-slate-600 rounded-lg">
            {children}
        </div>
    );
};