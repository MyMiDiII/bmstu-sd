create or replace
function get_event_state(eventDate date, startTime time, duration bigint,
						 beginReg  timestamp, endReg timestamp,
						 cancelled bool, deleted bool)
returns integer as
$$
declare
	curTime timestamp = now();
	beginEvent timestamp := eventDate + startTime;
	endEvent timestamp := beginEvent + (duration * interval '1 minute');
begin
	case
		when cancelled then
			return 5;
		when deleted then
			return 6;
		when curTime < beginReg then
			return 0;
		when beginReg <= curTime and curTime < endReg then
			return 1;
		when endReg <= curTime and curTime < beginEvent then
			return 2;
		when beginEvent <= curTime and curTime < endEvent then
			return 3;
		else
			return 4;
	end case;
end;
$$ language plpgsql;


select e."ID", e."Title", e."Date", e."StartTime", e."Duration",
	   e."Cost", e."Purchase", e."OrganizerID", e."VenueID",
	   e."Deleted", e."BeginRegistration", e."EndRegistration",
	   e."Cancelled", get_event_state(e."Date", e."StartTime", e."Duration",
	   								  e."BeginRegistration", e."EndRegistration",
	   								  e."Cancelled", e."Deleted")
from "Events" e
where e."Deleted" != true;