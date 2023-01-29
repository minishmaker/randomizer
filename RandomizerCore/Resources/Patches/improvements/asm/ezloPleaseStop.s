.thumb
push	{r4-r7,lr}
mov	r4,r0
ldrb	r0,[r4]
mov	r6,#0x0F
and	r6,r0
ldrb	r1,[r4,#1]
mov	r0,#0xF0

@check if this is ezlo dialogue
cmp	r6,#0x09	@event type
bne	notEzlo
ldrb	r3,[r4,#2]
cmp	r3,#0x0A	@ezlo dialogue type
bne	notEzlo

Ezlo:
pop	{r4-r7,pc}

notEzlo:
ldr	r3,=#0x804AB23
bx	r3
