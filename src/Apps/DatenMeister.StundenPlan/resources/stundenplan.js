var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "datenmeister/../Mof", "datenmeister/../Mof", "datenmeister/../forms/FormFactory", "DatenMeister.StundenPlan"], function (require, exports, Mof, Mof_1, FormFactory, StundenPlanTypes) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.WeeklyCalenderControl = exports.init = void 0;
    function init() {
        FormFactory.registerObjectForm(StundenPlanTypes._Forms.__SchedulerForm_Uri, () => {
            return new StundenPlanForm();
        });
        /**
         * Add the tests, only if the test framework is loaded
         */
        if (describe === undefined)
            return;
        describe('StundenPlanMeister Tests', function () {
            it('Create Empty Table', function () {
                const container = $('#test_area');
                const table = new WeeklyCalenderControl(container);
                table.createTable({
                    weeks: 4
                });
            });
            it('Test Event Manager', function () {
                const event1 = new Mof.DmObject();
                event1.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.onTuesday, true);
                event1.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, "09:00");
                event1.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekInterval, 1);
                event1.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekOffset, 0);
                event1.set(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, "Event1");
                const event2 = new Mof.DmObject();
                event2.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.onWednesday, true);
                event2.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, "09:00");
                event2.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekInterval, 2);
                event2.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekOffset, 0);
                event2.set(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, "Event2");
                const event3 = new Mof.DmObject();
                event3.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.onWednesday, true);
                event3.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, "09:00");
                event3.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekInterval, 2);
                event3.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekOffset, 1);
                event3.set(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, "Event3");
                const event4 = new Mof.DmObject();
                event4.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.onThursday, true);
                event4.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, "09:00");
                event4.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekInterval, 1);
                event4.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekOffset, 0);
                event4.set(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, "Event4");
                const event5 = new Mof.DmObject();
                event5.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.onThursday, true);
                event5.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, "08:00");
                event5.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekInterval, 1);
                event5.set(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekOffset, 0);
                event5.set(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, "Event5");
                const manager = new PeriodicEventManager([event1, event2, event3, event4, event5]);
                // Check single Tuesday
                const tuesdayFirstWeek = manager.getEventsOnWeekday(0, 2);
                chai.assert(tuesdayFirstWeek.length === 1, "Tuesday first length");
                chai.assert(tuesdayFirstWeek[0].get(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, Mof_1.ObjectType.String) === "Event1", "Event1 not on first Tuesday");
                const tuesdaySecondWeek = manager.getEventsOnWeekday(1, 2);
                chai.assert(tuesdaySecondWeek.length === 1, "Tuesday second week");
                chai.assert(tuesdaySecondWeek[0].get(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, Mof_1.ObjectType.String) === "Event1", "Event1 not on second tuesday");
                // Check odd and even weeks
                const wednesdayFirstWeek = manager.getEventsOnWeekday(0, 3);
                chai.assert(wednesdayFirstWeek.length === 1, "Wednesday first week length");
                chai.assert(wednesdayFirstWeek[0].get(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, Mof_1.ObjectType.String) === "Event2", "Event2 not on first Wednesday");
                const wednesdaySecondWeek = manager.getEventsOnWeekday(1, 3);
                chai.assert(wednesdaySecondWeek.length === 1, "Wednesday second week length");
                chai.assert(wednesdaySecondWeek[0].get(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, Mof_1.ObjectType.String) === "Event3", "Event3 not on second Wednesday");
                // Check Ordering
                const thursdayFirstWeek = manager.getEventsOnWeekday(0, 4);
                chai.assert(thursdayFirstWeek.length === 2, "Thursday first week");
                chai.assert(thursdayFirstWeek[0].get(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, Mof_1.ObjectType.String) === "Event5", "Event5 as first Thursday event");
                chai.assert(thursdayFirstWeek[1].get(StundenPlanTypes._Types._WeeklyPeriodicEvent._name_, Mof_1.ObjectType.String) === "Event4", "Event4 as second Thursday event");
            });
        });
    }
    exports.init = init;
    class StundenPlanForm {
        createFormByObject(parent, configuration) {
            return __awaiter(this, void 0, void 0, function* () {
                parent.append($("<span>Scheduler</span>"));
            });
        }
        refreshForm() {
            return;
        }
        storeFormValuesIntoDom(reuseExistingElement) {
            return __awaiter(this, void 0, void 0, function* () {
                return new Mof.DmObject();
            });
        }
    }
    class WeeklyCalenderControl {
        /**
         * Initializes a new instance of the object
         * @param container Container which is used to append the table
         */
        constructor(container) {
            /**
             * Stores each cell in the array
             */
            this.cells = new Array;
            this.container = container;
            this.isTableCreated = false;
            this.events = new Array;
        }
        createTable(configuration) {
            // Performs the default configuration, if not already set
            if (configuration.weeks === undefined) {
                configuration.weeks = 4;
            }
            // Deletes the existing content, if a table already has been created
            if (this.isTableCreated && this.table !== undefined) {
                this.table.remove();
                this.cells.length = 0;
            }
            // Now, we are live.. The table is created
            const table = $("<table></table>");
            for (let n = 0; n < configuration.weeks; n++) {
                const row = $("<tr></tr>");
                for (let day = 1; day <= 7; day++) {
                    const cell = $("<td></td>");
                    const weekDay = $("<span class='stundenplan-weekday'></span>");
                    weekDay.text(getWeekDay(day));
                    cell.append(weekDay);
                    this.cells.push(cell);
                    // Adds cell to the row
                    row.append(cell);
                }
                table.append(row);
            }
            this.container.append(table);
            this.table = table;
        }
        /**
         * Adds one item to the table.
         * Currently, if the table already has been created, we cannot add
         * an additional item to that
         * @param event Event that shall be added
         */
        addPeriodicEvent(event) {
            this.events.push(event);
        }
    }
    exports.WeeklyCalenderControl = WeeklyCalenderControl;
    /**
     * Some helper class which helps to organize the periodic events
     */
    class PeriodicEventManager {
        constructor(events) {
            this.events = events;
        }
        /**
         * Gets the events on a certain weekday within a certain week
         * @param week # of week to be evaluated
         * @param day Weekday to be evaluated
         */
        getEventsOnWeekday(week, day) {
            var _a;
            const foundEvents = new Array();
            for (let n in this.events) {
                const event = this.events[n];
                if (!isPeriodicEventApplicableOnWeekday(event, day)) {
                    continue;
                }
                const weekOffset = (_a = event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekOffset, Mof_1.ObjectType.Number)) !== null && _a !== void 0 ? _a : 0;
                let weekInterval = event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.weekInterval, Mof_1.ObjectType.Number);
                if (weekInterval === undefined || weekInterval < 1) {
                    weekInterval = 1;
                }
                // Checks, if we are in the right week! 
                if ((week - weekOffset) % weekInterval !== 0) {
                    continue;
                }
                // Ok, success, add it! 
                foundEvents.push(event);
            }
            // Now sort the entries according starting date
            foundEvents.sort((x, y) => {
                const startX = x.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, Mof_1.ObjectType.String);
                const startY = y.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.timeStart, Mof_1.ObjectType.String);
                return startX === startY ? 0 : startX < startY ? -1 : 1;
            });
            return foundEvents;
        }
    }
    /**
     * Checks whether the periodic event is applicable on a certain weekday
     * @param event Event to be checked
     * @param dayNumber Number of the day
     */
    function isPeriodicEventApplicableOnWeekday(event, dayNumber) {
        switch (dayNumber) {
            case 1:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onMonday, Mof_1.ObjectType.Boolean);
            case 2:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onTuesday, Mof_1.ObjectType.Boolean);
            case 3:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onWednesday, Mof_1.ObjectType.Boolean);
            case 4:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onThursday, Mof_1.ObjectType.Boolean);
            case 5:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onFriday, Mof_1.ObjectType.Boolean);
            case 6:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onSaturday, Mof_1.ObjectType.Boolean);
            case 7:
                return event.get(StundenPlanTypes._Types._WeeklyPeriodicEvent.onSunday, Mof_1.ObjectType.Boolean);
        }
    }
    function getWeekDay(dayNumber) {
        if (dayNumber < 1 || dayNumber > 7) {
            throw "Invalid argument, must be between 1 and 7";
        }
        switch (dayNumber) {
            case 1:
                return "Monday";
            case 2:
                return "Tuesday";
            case 3:
                return "Wednesday";
            case 4:
                return "Thursday";
            case 5:
                return "Friday";
            case 6:
                return "Saturday";
            case 7:
                return "Sunday";
        }
    }
});
//# sourceMappingURL=stundenplan.js.map