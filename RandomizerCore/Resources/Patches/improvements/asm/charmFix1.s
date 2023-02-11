.thumb
@some vanilla instructions
str	r0, [sp]
mov	r6, r1

@check if this is link, and not the first time his palette is loaded
ldrb	r1, [r0, #0x1A]
cmp	r1, #0
beq	vanilla @no palette yet, so we go vanilla
ldr	r1, =#0x3001160
cmp	r0, r1
bne	vanilla @not link, so we go vanilla

@instead of trying to find a new slot for the palette, simply use the one link already has
ldrb	r0, [r0, #0x1A]
lsl	r0, #32 - 4
lsr	r0, #32 - 4
b	end

vanilla:
mov	r0, r6
ldr	r3, =#0x801D140
mov	lr, r3
.short	0xF800

end:
ldr	r3, =#0x801D08E
mov	lr, r3
.short	0xF800