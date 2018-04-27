import { Command } from '../../services/Command';
import { Language } from '../language.model';

export class ChangePreferredLanguage extends Command {
    dataCollectorId: string;
    language: Language

    constructor() {
        super();
        this.type = 'CBS#VolunteerReporting.DataCollector-ChangePreferredLanguage+Command|Domain';
    }
}
