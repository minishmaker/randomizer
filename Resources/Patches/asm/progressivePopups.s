.thumb
push	{r4}
mov	r4,r2

@check if item was already collected
mov	r0,r2
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
@if not, popup
cmp	r0,#0
bne	popup

@check if the item is in the progressive list, if so, popup
ldr	r0,progressiveTable
mov	r1,#1
neg	r1,r1
loopbig:
ldr	r2,[r0]
cmp	r2,r1
beq	nopopup
loopsmall:
ldrb	r3,[r2]
cmp	r3,r4
beq	popup
add	r2,#1
cmp	r3,#0xFF
bne	loopsmall
nextbig:
add	r0,#8
b	loopbig

nopopup:
mov	r0,#0
b	end

popup:
mov	r0,#1

end:
pop	{r4}
pop	{r4,pc}

.align
.ltorg
progressiveTable:
@POIN progressiveTable
