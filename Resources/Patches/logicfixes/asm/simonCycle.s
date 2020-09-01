.thumb
push	{lr}
push	{r4-r6}
@check if simulation spot opened
ldr	r0,=#0x2002CB4
ldrb	r0,[r0,#0x00]
mov	r1,#0x40
and	r0,r1
cmp	r0,#0
bne	cycle
b	first

	@if not, always first fight
	first:
	mov	r6,#0
	b	end
	
	@otherwise cycle through fights
	cycle:
	ldr	r0,=#0x203FFE0
	ldrb	r6,[r0,#0x00]
	add	r6,#1
	cmp	r6,#8
	blo	notcap
		@capped at 7
		mov	r6,#0
	notcap:
	strb	r6,[r0,#0x00]
	b	end
	
end:
ldr	r3,=#0x804E082
mov	lr,r3
.short	0xF800
