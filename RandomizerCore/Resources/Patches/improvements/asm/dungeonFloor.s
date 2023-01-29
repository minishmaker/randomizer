.thumb
add	r0,r5
push	{r0-r3}
@check if dungeon map mode
ldr	r0,=#0x20344A4
ldrb	r0,[r0]
cmp	r0,#5
bne	vanilla

@check if we are in the map for our current location
ldr	r0,=#0x2033A90
ldrb	r0,[r0,#3]
ldr	r1,=#0x2032EE0
ldrb	r1,[r1]
lsl	r1,#2
ldr	r2,=#0x80528E8
ldr	r2,[r2]
add	r1,r2
ldrb	r1,[r1,#1]
sub	r1,#0x17
cmp	r0,r1
beq	match
pop	{r0-r3}
mov	r0,#0
b	end

match:
ldr	r3,=#0x801DBCC
mov	lr,r3
.short	0xF800
mov	r3,r0
pop	{r0-r2}
mov	r0,r3
pop	{r3}
b	end

vanilla:
pop	{r0-r3}
ldrb	r0,[r0]
end:
ldr	r3,=#0x80A4618
mov	lr,r3
mov	r3,#0
strb	r0,[r4,#3]
.short	0xF800
