export class DatesRangeFilter {
    constructor(from: Date = null, to: Date = null) {
        this.from = from;
        this.to = to;
    }

    public from: Date;
    public to: Date;
}